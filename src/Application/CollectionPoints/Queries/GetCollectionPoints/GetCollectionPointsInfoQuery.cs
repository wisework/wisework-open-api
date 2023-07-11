using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Common;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CollectionPoints.Queries.GetCollectionPoints;

public record GetCollectionPointsInfoQuery(int Id) : IRequest<CollectionPointInfo>
{
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

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
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }
        try
        {
            //todo:edit conpanyid หลังมีการทำ identity server
            var collectionpointInfo = (from cp in _context.DbSetConsentCollectionPoints
                                       where cp.CollectionPointId == request.Id && cp.CompanyId == request.authentication.CompanyID && cp.Status != Status.X.ToString()
                                       select new CollectionPointInfo
                                       {
                                           CollectionPointId = cp.CollectionPointId,
                                           CollectionPointName = cp.CollectionPoint,
                                           Guid = new Guid(cp.Guid),
                                           ExpiredDateTime = cp.KeepAliveData,
                                           CompanyId = (int)(long)cp.CompanyId,
                                           Version = cp.Version,
                                           Status = cp.Status,

                                       }).FirstOrDefault();

            #region website
            var websiteList = (from cp in _context.DbSetConsentCollectionPoints
                               join w in _context.DbSetConsentWebsite on cp.WebsiteId equals w.WebsiteId
                               where cp.CollectionPointId == request.Id && w.WebsiteId == cp.WebsiteId
                               select
                                   new CollectionPointInfoWebsiteObject
                                   {
                                       Id = w.WebsiteId,
                                       Description = w.Description,
                                       UrlHomePage = w.Url,
                                       UrlPolicyPage = w.Urlpolicy,
                                   }).FirstOrDefault();


            #endregion

            #region purpose list
            var purposeList = (from cp in _context.DbSetConsentCollectionPointItem
                               join p in _context.DbSetConsentPurpose on cp.PurposeId equals p.PurposeId
                               where cp.Status == Status.Active.ToString()
                               && p.Status == Status.Active.ToString()
                               && cp.CollectionPointId == request.Id
                               && p.CompanyId == request.authentication.CompanyID
                               && p.PurposeId == cp.PurposeId
                               select
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
                                   }).ToList();

            #endregion

            #region custom fields
            var customFieldInfo = (from cf in _context.DbSetConsentCollectionPointCustomFields
                                   join cpf in _context.DbSetConsent_CollectionPointCustomFieldConfig on cf.CollectionPointCustomFieldId equals cpf.CollectionPointCustomFieldId
                                   where collectionpointInfo.Guid.ToString() == cpf.CollectionPointGuid 
                                   && cf.CompanyId == request.authentication.CompanyID 
                                   && cf.Status != Status.X.ToString()
                                   
                                   select new CustomFields
                                   {
                                       Id = cf.CollectionPointCustomFieldId,
                                       IsRequired = cpf.Required.Value,
                                       Sequence = cpf.Sequence.Value,
                                       InputType = cf.Type,
                                       Title = cf.Description,
                                       Placeholder = cf.Placeholder,
                                       LengthLimit = cf.LengthLimit,
                                       MaxLines = cf.MaxLines,
                                       MinLines = cf.MinLines,
                                   }).ToList();
            #endregion

            #region page details
            var pageDetails = (from cp in _context.DbSetConsentPage
                               join p in _context.DbSetConsentCollectionPoints on cp.CollectionPointId equals p.CollectionPointId
                               where cp.Status == Status.Active.ToString() 
                               && cp.CollectionPointId == request.Id 
                               && cp.CompanyId == request.authentication.CompanyID
                               && cp.Status != Status.X.ToString()
                               

                               select
                               new CollectionPointPageDetail
                               {
                                   ThemeId = cp.ThemeId,
                                   AcceptCheckBoxText = cp.LabelCheckBoxAccept,
                                   BackgroundImageId = cp.BodyBgimage,
                                   BodyBottomDescriptionText = cp.BodyBottomDescription,
                                   BodyTopDescriptionText = cp.BodyTopDescription,
                                   CancelButtonText = cp.CancelButtonBgcolor,
                                   ConfirmButtonText = cp.OkbuttonBgcolor,
                                   HeaderBackgroundImageId = cp.HeaderBgimage,
                                   HeaderText = cp.HeaderLabel,
                                   LogoImageId = cp.HeaderLogo,
                                   PolicyUrlText = cp.LabelLinkToPolicy,
                                   PolicyUrl = cp.LabelLinkToPolicyUrl,
                                   RedirectUrl = cp.RedirectUrl,
                                   SuccessHeaderText = cp.HeaderLabelThankPage,
                                   SuccessDescriptionText = cp.BodyBottomDescription,
                                   SuccessButtonText = cp.ButtonThankpage,

                               }).FirstOrDefault();

            #endregion

            collectionpointInfo.CollectionPointInfoWebsiteObject = websiteList;
            collectionpointInfo.PurposeList = purposeList;
            collectionpointInfo.CustomFieldsList = customFieldInfo;
            collectionpointInfo.PageDetail = pageDetails;

            return collectionpointInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
       
    }

}