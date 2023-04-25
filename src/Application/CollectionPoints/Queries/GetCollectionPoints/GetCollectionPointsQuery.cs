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

namespace WW.Application.CollectionPoints.Queries.GetCollectionPoints;

public record GetCollectionPointsQuery : IRequest<PaginatedList<CollectionPointInfo>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

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

        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_CollectionPoint, CollectionPointInfo>();
            cfg.CreateMap<Consent_CollectionPointItem, GeneralConsentPurpose>();
            cfg.CreateMap<Companies, CollectionPointInfo>();
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        PaginatedList<CollectionPointInfo> model = 
            await _context.DbSetConsentCollectionPoints
            .Where(collectionPoint => collectionPoint.CompanyId == 1)
            .ProjectTo<CollectionPointInfo>(mapper.ConfigurationProvider).PaginatedListAsync(request.PageNumber, request.PageSize);

        
        var collectionPointIds = model.Items.Select(c => c.CollectionPointId);
        var collectionPointGuids = model.Items.Select(c => c.Guid);

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
                                    //PurposeType = p.PurposeType == 1 ? PurposeType.Consent.ToString() : PurposeType.Cookie.ToString(),
                                    Code = p.Code,
                                    Description = p.Description,
                                    WarningDescription = p.WarningDescription,
                                    PurposeCategoryId = p.PurposeCategoryId,
                                    ExpiredDateTime = Calulate.ExpiredDateTime(p.KeepAliveData,p.CreateDate),
                                    //Guid = p.Guid,
                                    Version = p.Version,
                                    Priority = cp.Priority,
                                    Status = p.Status,
                                    /*CreateBy = p.CreateBy,
                                    CreateDate = p.CreateDate.ToLocalTime(),
                                    UpdateBy = p.UpdateBy*/
                                })).ToList();

        var purposeLookup = purposeList.ToLookup(item => item.Key, item => item.Value);
        #endregion

        #region custom fields

        var customFieldsList = (from cf in _context.DbSetConsent_CollectionPointCustomFieldConfig
                               
                            //where collectionPointGuids.Contains(cf.CollectionPointGuid)
                            select new KeyValuePair<string, CollectionPointCustomFields>(cf.CollectionPointGuid,
                                new CollectionPointCustomFields
                                {
                                    Id = cf.CollectionPointCustomFieldId.Value,
                                    IsRequired = cf.Required.Value,
                                    Sequence = cf.Sequence.Value,
                                })).ToList();

        var customFieldsLookup = customFieldsList.ToLookup(item => item.Key, item => item.Value);
        #endregion

        #region page details
        var pageDetails = (from cp in _context.DbSetConsentPage
                           join p in _context.DbSetConsentCollectionPoints on cp.CollectionPointId equals p.CollectionPointId
                           where cp.Status == Status.Active.ToString() && collectionPointIds.Contains(cp.CollectionPointId)

                           select new KeyValuePair<int, CollectionPointPageDetail>(cp.CollectionPointId,
                           new CollectionPointPageDetail
                           {
                               /*AcceptCheckBoxLabel = cp.LabelCheckBoxAccept,
                               AcceptCheckBoxLabelFontColor = cp.LabelCheckBoxAcceptFontColor,
                               BodyBackgroundColor = cp.BodyBgcolor,*/
                               // BodyBackgroundId 
                               /*BodyBackground = cp.BodyBgcolor,
                               BodyBottomDescription = cp.BodyBottomDescription,
                               BodyBottomDescriptionFontColor = cp.BodyBottomDescriptionFontColor,
                               BodyTopDescription = cp.BodyTopDescription,
                               BodyTopDerscriptionFontColor = cp.BodyTopDerscriptionFontColor,
                               CancelButtonBackgroundColor = cp.CancelButtonBgcolor,
                               CancelButtonFontColor = cp.CancelButtonFontColor,
                               CancelButtonLabel = cp.LabelActionCancel,*/
                               //ConfirmButtonLabel 
                               //HeaderBackgroundColor = cp.HeaderBgcolor,
                               //HeaderBackgroundId 
                               // HeaderBackground
                               /*HeaderFontColor = cp.HeaderFontColor,
                               HeaderLabel = cp.HeaderLabel,*/
                               //Logo,
                               //LogoId,
                               /*OkButtonBackgroundColor = cp.OkbuttonBgcolor,
                               OkButtonFontColor = cp.OkbuttonFontColor,*/
                               PolicyUrl = cp.LabelLinkToPolicyUrl,
                               /*PolicyUrlLabel = cp.LabelLinkToPolicyUrl,
                               PurposeAcceptLabel = cp.LabelPurposeActionAgree,
                               PolicyUrlLabelFontColor = cp.LabelLinkToPolicyFontColor,
                               PurposeRejectLabel = cp.LabelPurposeActionNotAgree,*/
                               RedirectUrl = cp.RedirectUrl,
                               /*SuccessHeaderLabel = cp.HeaderLabelThankPage,
                               SuccessDescription = cp.ShortDescriptionThankPage,
                               SuccessButtonLabel = cp.ButtonThankpage*/
                           })).ToList();

        #endregion

        #region get info collection point
        var collectionpointInfo = (from cp in _context.DbSetConsentCollectionPoints
                                   join uCreate in _context.DbSetUser on cp.CreateBy equals uCreate.CreateBy
                                   join uUpdate in _context.DbSetUser on cp.CreateBy equals uUpdate.UpdateBy
                                   join c in _context.DbSetCompanies on cp.CompanyId equals (int)(long)c.CompanyId
                                   join w in _context.DbSetConsentWebsite on (int)(long)c.CompanyId equals w.CompanyId
                                   where collectionPointIds.Contains(cp.CollectionPointId) && cp.CompanyId == 1 && cp.Status != Status.X.ToString()
                                   select new KeyValuePair<int, CollectionPointInfo>(cp.CollectionPointId,
                                   new CollectionPointInfo
                                   {
                                       CollectionPointId = cp.CollectionPointId,
                                       /*CollectionPoint = cp.CollectionPoint,
                                       WebsiteId = cp.WebsiteId,
                                       AccessToken = c.AccessToken,
                                       Guid = cp.Guid,
                                       WebsiteDescription = w.Description,
                                       WebsiteUrl = w.Url,
                                       WebsitePolicy = w.Urlpolicy,
                                       Description = cp.Description,
                                       Script = cp.Script,*/
                                       CompanyId = (int)(long)c.CompanyId,
                                       Version = c.Version,
                                       Status = c.Status,
                                       /*StatusDisplay = c.Status == "Active" ? Status.Active.ToString() : Status.Inactive.ToString(),
                                       CreateBy = c.CreateBy,
                                       CreateByDisplay = String.Format("{0} {1}", uCreate.FirstName, uCreate.LastName),
                                       CreateDate = c.CreateDate.ToLocalTime(),
                                       CreateDateDisplay = c.CreateDate.LocalDateTime.ToShortDateString(),
                                       UpdateBy = c.UpdateBy,
                                       UpdateByDisplay = String.Format("{0} {1}", uUpdate.FirstName, uUpdate.LastName),
                                       IsStatus = c.Status == "Active" ? true : false*/
                                   })).ToList();
      

        #endregion

        foreach (var item in model.Items)
        {
            item.PurposeList = purposeLookup[item.CollectionPointId.Value].ToList();
            //item.CustomFieldsList = customFieldsLookup[item.Guid].ToList();
            item.PageDetail = pageDetails.Where(pd => pd.Key == item.CollectionPointId.Value).Select(x=>x.Value).FirstOrDefault();
            /*item.AccessToken = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.AccessToken).FirstOrDefault();
            item.WebsiteId = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.WebsiteId).FirstOrDefault();
            item.WebsiteUrl = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.WebsiteUrl).FirstOrDefault();
            item.WebsiteDescription = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.WebsiteDescription).FirstOrDefault();
            item.WebsitePolicy = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.WebsitePolicy).FirstOrDefault();
            item.Description = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Description).FirstOrDefault();
            item.Script = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Script).FirstOrDefault();*/
            item.CompanyId = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CompanyId).FirstOrDefault();
            item.Version = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Version).FirstOrDefault();
            item.Status = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.Status).FirstOrDefault();
            /*item.StatusDisplay = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.StatusDisplay).FirstOrDefault();
            item.CreateBy = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateBy).FirstOrDefault();
            item.CreateByDisplay = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateByDisplay).FirstOrDefault();
            item.CreateDate = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateDate).FirstOrDefault();
            item.CreateDateDisplay = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.CreateDateDisplay).FirstOrDefault();
            item.UpdateBy = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.UpdateBy).FirstOrDefault();
            item.UpdateByDisplay = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.UpdateByDisplay).FirstOrDefault();
            item.IsStatus = collectionpointInfo.Where(x => x.Key == item.CollectionPointId.Value).Select(selector: c => c.Value.IsStatus).FirstOrDefault();*/
        }

        return model;
    }

}