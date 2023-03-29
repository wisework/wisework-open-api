using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CollectionPoints.Queries.GetCollectionPoints;

public record GetCollectionPointsInfoQuery(int Id) : IRequest<CollectionPointInfo>;

public class GetCollectionPointsInfoQueryHandler : IRequestHandler<GetCollectionPointsInfoQuery, CollectionPointInfo>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCollectionPointsInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CollectionPointInfo> Handle(GetCollectionPointsInfoQuery request, CancellationToken cancellationToken)
    {

        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Consent_CollectionPoint, CollectionPointInfo>();
        });

        Mapper mapper = new Mapper(config);

        //todo:edit conpanyid หลังมีการทำ identity server
        var collectionpointInfo = (from cp in _context.DbSetConsentCollectionPoints
                                   join uCreate in _context.DbSetUser on cp.CreateBy equals uCreate.CreateBy
                                   join uUpdate in _context.DbSetUser on cp.CreateBy equals uUpdate.UpdateBy
                                   join c in _context.DbSetCompanies on cp.CompanyId equals (int)(long)c.CompanyId
                                   join w in _context.DbSetConsentWebsite on (int)(long)c.CompanyId equals w.CompanyId
                                   where cp.CollectionPointId == request.Id && cp.CompanyId == 1 && cp.Status != Status.X.ToString()
                                   select new CollectionPointInfo
                                   {
                                       CollectionPointId = cp.CollectionPointId,
                                       CollectionPoint = cp.CollectionPoint,
                                       WebsiteId = cp.WebsiteId,
                                       AccessToken = c.AccessToken,
                                       Guid = cp.Guid,
                                       WebsiteDescription = w.Description,
                                       WebsiteUrl = w.Url,
                                       WebsitePolicy = w.Urlpolicy,
                                       Description = cp.Description,
                                       Script = cp.Script,
                                       CompanyId = (int)(long)c.CompanyId,
                                       Version = c.Version,
                                       Status = c.Status,
                                       StatusDisplay = c.Status == "Active" ? Status.Active.ToString() :Status.Inactive.ToString(),
                                       CreateBy = c.CreateBy,
                                       CreateByDisplay = String.Format("{0} {1}", uCreate.FirstName, uCreate.LastName),
                                       CreateDate = c.CreateDate.ToLocalTime(),
                                       CreateDateDisplay = c.CreateDate.LocalDateTime.ToShortDateString(),
                                       UpdateBy = c.UpdateBy,
                                       UpdateByDisplay = String.Format("{0} {1}", uUpdate.FirstName, uUpdate.LastName),
                                       IsStatus = c.Status == "Active" ? true : false
                                   }).FirstOrDefault();

        
        #region purpose list
        var purposeList = (from cp in _context.DbSetConsentCollectionPointItem
                           join p in _context.DbSetConsentPurpose on cp.PurposeId equals p.PurposeId
                           where cp.Status == Status.Active.ToString()
                           && p.Status == Status.Active.ToString()
                           && cp.CollectionPointId == request.Id
                           select
                               new GeneralConsentPurpose
                               {
                                   PurposeId = p.PurposeId,
                                   PurposeType = p.PurposeType == 1 ? PurposeType.Consent.ToString() : PurposeType.Cookie.ToString(),
                                   Code = p.Code,
                                   Description = p.Description,
                                   WarningDescription = p.WarningDescription,
                                   PurposeCategoryId = p.PurposeCategoryId,
                                   ExpiredDateTime = Calulate.ExpiredDateTime(p.KeepAliveData, p.CreateDate),
                                   Guid = p.Guid,
                                   Version = p.Version,
                                   Priority = cp.Priority,
                                   Status = p.Status,
                                   CompanyId = p.CompanyId,
                                   CreateBy = p.CreateBy,
                                   CreateDate = p.CreateDate.ToLocalTime(),
                                   UpdateBy = p.UpdateBy,
                                   UpdateDate = p.UpdateDate.ToLocalTime(),
                                   Active = p.Status == Status.Active.ToString()?true:false

                               }).ToList();

        
        #endregion

        #region custom fields

        var customFieldsList = (from cf in _context.DbSetConsent_CollectionPointCustomFieldConfig

                                where collectionpointInfo.Guid == cf.CollectionPointGuid
                                select 
                                    new CollectionPointCustomFields
                                    {
                                        Id = cf.CollectionPointCustomFieldId.Value,
                                        IsRequired = cf.Required.Value,
                                        Sequence = cf.Sequence.Value,
                                    }).ToList();


        #endregion

        #region page details
        var pageDetails = (from cp in _context.DbSetConsentPage
                           join p in _context.DbSetConsentCollectionPoints on cp.CollectionPointId equals p.CollectionPointId
                           where cp.Status == Status.Active.ToString() && cp.CollectionPointId == collectionpointInfo.CollectionPointId

                           select
                           new CollectionPointPageDetail
                           {
                               AcceptCheckBoxLabel = cp.LabelCheckBoxAccept,
                               AcceptCheckBoxLabelFontColor = cp.LabelCheckBoxAcceptFontColor,
                               BodyBackgroundColor = cp.BodyBgcolor,
                               // BodyBackgroundId 
                               BodyBackground = cp.BodyBgcolor,
                               BodyBottomDescription = cp.BodyBottomDescription,
                               BodyBottomDescriptionFontColor = cp.BodyBottomDescriptionFontColor,
                               BodyTopDescription = cp.BodyTopDescription,
                               BodyTopDerscriptionFontColor = cp.BodyTopDerscriptionFontColor,
                               CancelButtonBackgroundColor = cp.CancelButtonBgcolor,
                               CancelButtonFontColor = cp.CancelButtonFontColor,
                               CancelButtonLabel = cp.LabelActionCancel,
                               //ConfirmButtonLabel 
                               HeaderBackgroundColor = cp.HeaderBgcolor,
                               //HeaderBackgroundId 
                               // HeaderBackground
                               HeaderFontColor = cp.HeaderFontColor,
                               HeaderLabel = cp.HeaderLabel,
                               //Logo,
                               //LogoId,
                               OkButtonBackgroundColor = cp.OkbuttonBgcolor,
                               OkButtonFontColor = cp.OkbuttonFontColor,
                               PolicyUrl = cp.LabelLinkToPolicyUrl,
                               PolicyUrlLabel = cp.LabelLinkToPolicyUrl,
                               PurposeAcceptLabel = cp.LabelPurposeActionAgree,
                               PolicyUrlLabelFontColor = cp.LabelLinkToPolicyFontColor,
                               PurposeRejectLabel = cp.LabelPurposeActionNotAgree,
                               RedirectUrl = cp.RedirectUrl,
                               SuccessHeaderLabel = cp.HeaderLabelThankPage,
                               SuccessDescription = cp.ShortDescriptionThankPage,
                               SuccessButtonLabel = cp.ButtonThankpage
                           }).FirstOrDefault();

        #endregion

        collectionpointInfo.PurposeList = purposeList;
        collectionpointInfo.CustomFieldsList = customFieldsList;
        collectionpointInfo.PageDetail = pageDetails;

        return collectionpointInfo;
    }

}