using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Exceptions;
using WW.Application.Common.Interfaces;
using WW.Application.Common.Models;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CollectionPoints.Commands.CreateCollectionPoint;

public record CreateCollectionPointCommand : IRequest<CollectionPointObject>
{
    public string CollectionPointName { get; init; }
    public List<CollectionPointConsentKeyIdentifier> ConsentKeyIdentifier { get; init; }
    public List<CollectionPointCustomFields> CustomFieldsList { get; init; }
    public string ExpirationPeriod { get; init; }
    public string Language { get; init; }
    public CollectionPointPageDetail PageDetail { get; init; }
    public List<CollectionPointPurpose> PurposesList { get; init; }
    public int WebsiteId { get; init; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateCollectionPointCommand, CollectionPointObject>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CollectionPointObject> Handle(CreateCollectionPointCommand request, CancellationToken cancellationToken)
    {
        var entity = new Consent_CollectionPoint();
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }
        try
        {
            #region add collection point and PrimaryKey

            
            entity.CollectionPoint = request.CollectionPointName;
            entity.WebsiteId = request.WebsiteId;
            entity.KeepAliveData = request.ExpirationPeriod;
            entity.Language = request.Language;
            //todo: change affter identity server
            entity.CompanyId = request.authentication.CompanyID;
            entity.Description = "";
            entity.Script = "";
            entity.CreateBy = request.authentication.UserID;
            entity.UpdateBy = request.authentication.UserID;
            entity.UpdateDate = DateTime.Now;
            entity.CreateDate = DateTime.Now;

            entity.Status = Status.Active.ToString();
            entity.Version = 1;

            foreach (var key in request.ConsentKeyIdentifier)
            {
                switch (key.Code)
                {
                    case "Uid":
                        entity.ActiveConsentUidpk = key.IsPrimaryKey;
                        entity.ActiveConsentUidrequired = key.IsRequired;
                        break;
                    case "IdCardNumber":
                        entity.ActiveConsentCardNumberPk = key.IsPrimaryKey;
                        entity.ActiveConsentCardNumberRequired = key.IsRequired;
                        break;
                    case "Email":
                        entity.ActiveConsentEmailPk = key.IsPrimaryKey;
                        entity.ActiveConsentEmailRequired = key.IsRequired;
                        break;
                    case "Fullname":
                        entity.ActiveConsentNamePk = key.IsPrimaryKey;
                        entity.ActiveConsentNameRequired = key.IsRequired;
                        break;
                    case "PhoneNumber":
                        entity.ActiveConsentTelPk = key.IsPrimaryKey;
                        entity.ActiveConsentTelRequired = key.IsRequired;
                        break;
                }
            }


            _context.DbSetConsentCollectionPoints.Add(entity);
            #endregion

            #region add purpose
            foreach (var purpose in request.PurposesList)
            {
                var purposeEntity = new Consent_CollectionPointItem();
                // id collecion point
                purposeEntity.CollectionPointId = entity.CollectionPointId;
                purposeEntity.Priority = purpose.Priority;
                purposeEntity.SectionInfoId = purpose.SectionId;

                //todo:change affter identity server
                purposeEntity.CreateBy = request.authentication.UserID;
                purposeEntity.UpdateBy = request.authentication.UserID;
                purposeEntity.UpdateDate = DateTime.Now;
                purposeEntity.CreateDate = DateTime.Now;
                purposeEntity.Status = Status.Active.ToString();
                purposeEntity.Version = 1;
                _context.DbSetConsentCollectionPointItem.Add(purposeEntity);

            }
            #endregion

            #region add customfileds
            foreach (var customFields in request.CustomFieldsList)
            {
                var customFieldsEntity = new Consent_CollectionPointCustomFieldConfig();
                // id collecion point
                customFieldsEntity.CollectionPointCustomFieldId = customFields.Id;
                customFieldsEntity.Required = customFields.IsRequired;
                customFieldsEntity.Sequence = customFields.Sequence;
                _context.DbSetConsent_CollectionPointCustomFieldConfig.Add(customFieldsEntity);

            }
            #endregion

            #region add consent page
            var pageDetail = new Consent_Page();
            pageDetail.ThemeId = request.PageDetail.ThemeId;
            pageDetail.CollectionPointId = entity.CollectionPointId;
            pageDetail.LabelCheckBoxAccept = request.PageDetail.AcceptCheckBoxText;
            pageDetail.BodyBgimage = request.PageDetail.BackgroundImageId;
            pageDetail.BodyBottomDescription = request.PageDetail.BodyBottomDescriptionText;
            pageDetail.BodyTopDescription = request.PageDetail.BodyTopDescriptionText;
            pageDetail.LabelActionCancel = request.PageDetail.CancelButtonText;
            pageDetail.LabelActionOk = request.PageDetail.ConfirmButtonText;
            pageDetail.HeaderBgimage = request.PageDetail.HeaderBackgroundImageId;
            pageDetail.HeaderLabel = request.PageDetail.HeaderText;
            pageDetail.HeaderLogo = request.PageDetail.LogoImageId;
            pageDetail.LabelLinkToPolicyUrl = request.PageDetail.PolicyUrl;
            pageDetail.LabelLinkToPolicy = request.PageDetail.PolicyUrlText;
            pageDetail.UrlconsentPage = "https://demo-pdpa.thewiseworks.com/CMSConsent/ConsentPage?code=P9hXBHMlcE6VxMr9yfTDU%2FkPePtS7S%2FYT4wpimQ1CqYNHdtHZHtnyaFbXHCGJNFb85UJKuzTKJvKOtYsL%2BNOsbGjwJMWYe%2BuvDDzj5YStofWtLAXhxsRjWhFaOs3zPQBDVdTf%2Bo7MpkARrY7QnkIstfxWTkL6a3l1lTkQ0ZFX6og4w72Ht7bbVnYOdNtlU7KDh3au%2Fxuiag8mlN%2FRNqRGlOiFaT7%2FOUSuWUsyZYtldtADgAr3Prf2XjciZ2Jh%2BAMiFs7mPM75rSuBZJFNeYibDwBpyPFHon5L599uKlAK7TeuDkTReB2TwcvWgWsLnUDLTZ1Vfis%2FgfKOYQwf7SwHqDIHioyPnoVQv%2B74IRVr1q7oie7B1gn%2FM3cQ9SAHDCa";
            pageDetail.RedirectUrl = request.PageDetail.RedirectUrl;
            pageDetail.HeaderLabelThankPage = request.PageDetail.SuccessHeaderText;
            pageDetail.ShortDescriptionThankPage = request.PageDetail.SuccessDescriptionText;
            pageDetail.ButtonThankpage = request.PageDetail.SuccessButtonText;
            pageDetail.LabelPurposeActionAgree = "";
            pageDetail.LabelPurposeActionNotAgree = "";

            //todo:change affter identity server
            pageDetail.LanguageCulture = request.Language;
            pageDetail.Status = Status.Active.ToString();
            pageDetail.Version = 1;
            pageDetail.CompanyId = request.authentication.CompanyID;
            pageDetail.CreateBy = request.authentication.UserID;
            pageDetail.UpdateBy = request.authentication.UserID;
            pageDetail.UpdateDate = DateTime.Now;
            pageDetail.CreateDate = DateTime.Now;


            _context.DbSetConsentPage.Add(pageDetail);

            #endregion

            #region website
            var websiteList = (from cp in _context.DbSetConsentCollectionPoints
                               join w in _context.DbSetConsentWebsite on cp.WebsiteId equals w.WebsiteId
                               where cp.CollectionPointId == request.WebsiteId
                               select
                                   new CollectionPointInfoWebsiteObject
                                   {
                                       Id = w.WebsiteId,
                                       Description = w.Description,
                                       UrlHomePage = w.Url,
                                       UrlPolicyPage = w.Urlpolicy,
                                   }).FirstOrDefault();


            #endregion

            await _context.SaveChangesAsync(cancellationToken);

            var collectionpointInfo = new CollectionPointObject
            {
                Id = entity.CollectionPointId,
                Name = entity.CollectionPoint,
                ConsentKeyIdentifier = request.ConsentKeyIdentifier,
                CustomFieldsList = request.CustomFieldsList,
                ExpirationPeriod = request.ExpirationPeriod,
                Language = request.Language,
                PageDetail = request.PageDetail,
                PurposeList = request.PurposesList,
                //Website = websiteList,

            };

            return collectionpointInfo;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

    }
}
