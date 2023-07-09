using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Mappings;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using Wisework.ConsentManagementSystem.Api;
using WW.Domain.Enums;
using WW.Domain.Common;
using System.Text.Json.Serialization;
using WW.Application.Common.Exceptions;

namespace WW.Application.CollectionPoints.Queries.GetCollectionPoints;

public record GetCollectionPointsQuery : IRequest<PaginatedList<CollectionPointInfo>>
{
    public int Offset { get; init; } = 1;
    public int Limit { get; init; } = 10;
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }

}


public class GetCollectionPointsQueryHandler : IRequestHandler<GetCollectionPointsQuery, PaginatedList<CollectionPointInfo>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCollectionPointsQueryHandler(IApplicationDbContext context, IMapper mapper) 
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CollectionPointInfo>> Handle(GetCollectionPointsQuery request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }
        try
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Consent_CollectionPoint, CollectionPointInfo>()
                .ForMember(d => d.CollectionPointId, a => a.MapFrom(s => s.CollectionPointId));
                cfg.CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s)
                ? (Guid?)null : Guid.Parse(s));
            });

            Mapper mapper = new Mapper(config);

            //todo:edit conpanyid หลังมีการทำ identity server
            PaginatedList<CollectionPointInfo> model =
              await _context.DbSetConsentCollectionPoints
              .Where(collectionPoint => collectionPoint.CompanyId == request.authentication.CompanyID && collectionPoint.Status == Status.Active.ToString())
              .ProjectTo<CollectionPointInfo>(mapper.ConfigurationProvider).PaginatedListAsync(request.Offset, request.Limit);

            var completelyConsentForm = (from c in _context.DbSetConsentCollectionPoints
                                         join p in _context.DbSetConsentPage on c.CollectionPointId equals p.CollectionPointId
                                         where c.CompanyId == request.authentication.CompanyID 
                                         && c.Status == Status.Active.ToString() 
                                         && p.CompanyId == request.authentication.CompanyID 
                                         && p.Status == Status.Active.ToString()
                                         select new
                                         {
                                             ConsentId = c.CollectionPointId,
                                             ConsentTitle = c.CollectionPoint,
                                             CompanyId = c.CompanyId,
                                             ConsentPageId = p.PageId,
                                         }).ToList();

            var collectionPointIds = model.Items.Select(c => c.CollectionPointId);
            var collectionPointGuids = model.Items.Select(c => c.Guid);

            #region get info collection point
            var collectionpointInfo = (from cp in _context.DbSetConsentCollectionPoints
                                       where collectionPointIds.Contains(cp.CollectionPointId) 
                                       && cp.CompanyId == request.authentication.CompanyID 
                                       && cp.Status != Status.X.ToString()
                                       select new KeyValuePair<int, CollectionPointInfo>(cp.CollectionPointId,
                                       new CollectionPointInfo
                                       {
                                           CollectionPointId = cp.CollectionPointId,
                                           CollectionPointName = cp.CollectionPoint,
                                           Guid = new Guid(cp.Guid),
                                           ExpiredDateTime = cp.KeepAliveData,
                                           CompanyId = (int)(long)cp.CompanyId,
                                           Version = cp.Version,
                                           Status = cp.Status,
                                       })).ToList();
            #endregion

            #region website
            var websiteList = (from cp in _context.DbSetConsentCollectionPoints
                               join w in _context.DbSetConsentWebsite on cp.WebsiteId equals w.WebsiteId
                               where collectionPointIds.Contains(cp.CollectionPointId)
                               select new KeyValuePair<int, CollectionPointInfoWebsiteObject>(cp.CollectionPointId,
                                   new CollectionPointInfoWebsiteObject
                                   {
                                       Id = w.WebsiteId,
                                       Description = w.Description,
                                       UrlHomePage = w.Url,
                                       UrlPolicyPage = w.Urlpolicy,
                                   })).ToList();

            var websiteLookup = websiteList.ToLookup(item => item.Key, item => item.Value);
            #endregion

            #region purpose list
            var purposeList = (from cp in _context.DbSetConsentCollectionPointItem
                               join p in _context.DbSetConsentPurpose on cp.PurposeId equals p.PurposeId
                               where cp.Status == Status.Active.ToString()
                               && p.Status == Status.Active.ToString()
                               && collectionPointIds.Contains(cp.CollectionPointId)
                               select new KeyValuePair<int, GeneralConsentPurpose>(cp.CollectionPointId,
                                   new GeneralConsentPurpose
                                   {
                                       PurposeId = p.PurposeId,
                                       CompanyId = p.CompanyId,
                                       Code = p.Code,
                                       Description = p.Description,
                                       WarningDescription = p.WarningDescription,
                                       PurposeCategoryId = p.PurposeCategoryId,
                                       ExpiredDateTime = Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate),
                                       Guid = new Guid(p.Guid),
                                       Version = p.Version,
                                       Priority = cp.Priority,
                                       Status = p.Status
                                   })).ToList();

            var purposeLookup = purposeList.ToLookup(item => item.Key, item => item.Value);
            #endregion

            #region custom fields

            var customFieldInfo = (from cp in _context.DbSetConsentCollectionPoints
                                   join cff in _context.DbSetConsent_CollectionPointCustomFieldConfig on cp.Guid equals cff.CollectionPointGuid into cffJoin
                                   from cff in cffJoin.DefaultIfEmpty()
                                   join cpc in _context.DbSetConsentCollectionPointCustomFields on cff.CollectionPointCustomFieldId equals cpc.CollectionPointCustomFieldId into cpcJoin
                                   from cpc in cpcJoin.DefaultIfEmpty()
                                   where collectionPointIds.Contains(cp.CollectionPointId) 
                                   && cp.Status == "Active" 
                                   && cp.CompanyId == request.authentication.CompanyID 
                                   && cpc.Status == "Active"
                                   && cpc.CompanyId == request.authentication.CompanyID
                                   select new KeyValuePair<int, CustomFields>(cp.CollectionPointId,
                                   new CustomFields
                                   {
                                       Id = cpc.CollectionPointCustomFieldId,
                                       IsRequired = cff.Required.Value,
                                       Sequence = cff.Sequence.Value,
                                       InputType = cpc.Type,
                                       Title = cpc.Description,
                                       Placeholder = cpc.Placeholder,
                                       LengthLimit = cpc.LengthLimit,
                                       MaxLines = cpc.MaxLines,
                                       MinLines = cpc.MinLines,
                                   })).ToList();

            var customFieldsLookup = customFieldInfo.ToLookup(item => item.Key, item => item.Value);
            #endregion

            #region page details
            var pageDetails = (from cp in _context.DbSetConsentCollectionPoints
                               join p in _context.DbSetConsentPage
                               on cp.CollectionPointId equals p.CollectionPointId
                               where cp.Status == Status.Active.ToString()
                               && collectionPointIds.Contains(cp.CollectionPointId)
                               && cp.CompanyId == request.authentication.CompanyID 
                               && cp.Status != Status.X.ToString()
                               && p.Status != Status.X.ToString()
                               && p.CompanyId == request.authentication.CompanyID

                               select new KeyValuePair<int, CollectionPointPageDetail>(cp.CollectionPointId,
                               new CollectionPointPageDetail
                               {
                                   AcceptCheckBoxText = p.LabelCheckBoxAccept,
                                   BackgroundImageId = p.BodyBgimage,
                                   BodyBottomDescriptionText = p.BodyBottomDescription,
                                   BodyTopDescriptionText = p.BodyTopDescription,
                                   CancelButtonText = p.CancelButtonBgcolor,
                                   ConfirmButtonText = p.OkbuttonBgcolor,
                                   HeaderBackgroundImageId = p.HeaderBgimage,
                                   HeaderText = p.HeaderLabel,
                                   LogoImageId = p.HeaderLogo,
                                   PolicyUrlText = p.LabelLinkToPolicy,
                                   PolicyUrl = p.LabelLinkToPolicyUrl,
                                   RedirectUrl = p.RedirectUrl,
                                   SuccessHeaderText = p.HeaderLabelThankPage,
                                   SuccessDescriptionText = p.BodyBottomDescription,
                                   SuccessButtonText = p.ButtonThankpage,

                               })).ToList();

            var pageDetailsLookup = pageDetails.ToLookup(item => item.Key, item => item.Value);
            #endregion



            foreach (var item in model.Items)
            {
                item.CollectionPointId = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CollectionPointId).FirstOrDefault();
                item.CollectionPointName = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CollectionPointName).FirstOrDefault();
                item.Guid = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Guid).FirstOrDefault();
                item.CollectionPointInfoWebsiteObject = websiteLookup[item.CollectionPointId.Value].FirstOrDefault();  //JSON
                item.PurposeList = purposeLookup[item.CollectionPointId.Value].ToList();
                item.CustomFieldsList = customFieldsLookup[item.CollectionPointId.Value].ToList();
                item.ExpiredDateTime = model.Items.Select(c => c.ExpiredDateTime).ToString();
                item.CompanyId = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CompanyId).FirstOrDefault();
                item.Version = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Version).FirstOrDefault();
                item.Status = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Status).FirstOrDefault();
                item.PageDetail = pageDetailsLookup[item.CollectionPointId.Value].FirstOrDefault();

            }

            return model;
        }
        catch
        (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
    
    }

}

