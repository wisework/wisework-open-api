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
using WW.Domain.Exceptions;

namespace WW.Application.CollectionPoints.Commands.UpdateCollectionPoint;
public record UpdateCollectionPointCommand : IRequest<int>
{
    public int Id { get; set; }
    public string CollectionPointName { get; init; }
    public List<CollectionPointConsentKeyIdentifier> ConsentKeyIdentifier { get; init; }
    public List<CollectionPointCustomFields> CustomFieldsList { get; init; }
    public string ExpirationPeriod { get; init; }
    public string Language { get; init; }
    public CollectionPointPageDetail PageDetail { get; init; }
    public List<CollectionPointPurpose> PurposesList { get; init; }
    public int WebsiteId { get; init; }
    public int Version { get; init; }
    [JsonIgnore]
    public AuthenticationModel? authentication { get; set; }
}
public class UpdateCollectionPointCommandHandler : IRequestHandler<UpdateCollectionPointCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateCollectionPointCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateCollectionPointCommand request, CancellationToken cancellationToken)
    {
        if (request.authentication == null)
        {
            throw new UnauthorizedAccessException();
        }
        try
        {
            #region add collection point and PrimaryKey
            var entity = new Consent_CollectionPoint();
            entity.CollectionPoint = request.CollectionPointName;
            entity.WebsiteId = request.WebsiteId;
            entity.KeepAliveData = request.ExpirationPeriod;
            entity.Language = request.Language;
            entity.CompanyId = request.authentication.CompanyID;
            entity.Description = "";
            entity.Script = "";
            //todo:change affter identity server
            entity.CreateBy = request.authentication.UserID;
            entity.UpdateBy = request.authentication.UserID;
            entity.UpdateDate = DateTime.Now;
            entity.CreateDate = DateTime.Now;

            entity.Status = Status.Active.ToString();
            entity.Version += entity.Version;

            _context.DbSetConsentCollectionPoints.Update(entity);
            #endregion

            #region update purpose 
            //todo: company id get form identity server
            //(รายการเก่า update status เป็น x แล้ว insert รายการใหม่)

            var queryListCollectionPointItem = from collectionPoint in _context.DbSetConsentCollectionPointItem
                                               where collectionPoint.CollectionPointId == request.Id && collectionPoint.Version == request.Version && collectionPoint.CompanyId == request.authentication.CompanyID
                                               select collectionPoint;

            if (queryListCollectionPointItem == null)
            {
                throw new UnsupportedVersionException(request.Id);
            }

            foreach (var item in queryListCollectionPointItem)
            {
                item.Status = Status.X.ToString();
            }

            foreach (var purpose in request.PurposesList)
            {
                var purposeEntity = new Consent_CollectionPointItem();
                purposeEntity.CollectionPointId = entity.CollectionPointId;
                purposeEntity.Priority = purpose.Priority;
                purposeEntity.SectionInfoId = purpose.SectionId;

                //todo:change affter identity server
                purposeEntity.CreateBy = request.authentication.UserID;
                purposeEntity.UpdateBy = request.authentication.UserID;
                purposeEntity.UpdateDate = DateTime.Now;
                purposeEntity.CreateDate = DateTime.Now;
                purposeEntity.Status = Status.Active.ToString();
                purposeEntity.Version += purposeEntity.Version;
                _context.DbSetConsentCollectionPointItem.Update(purposeEntity);

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
                _context.DbSetConsent_CollectionPointCustomFieldConfig.Update(customFieldsEntity);

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
            pageDetail.UrlconsentPage = "https://demo-pdpa.thewiseworks.com/CMSConsent/ConsentPage?code=P9hXBHMlcE6VxMr9yfTDU%2FkPePtS7S%2FYT4wpimQ1CqYNHdtHZHtnyaFbXHCGJNFb85UJKuzTKJvKOtYsL%2BNOsbGjwJMWYe%2BuvDDzj5YStofWtLAXhxsRjWhFaOs3zPQBDVdTf%2Bo7MpkARrY7QnkIstfxWTkL6a3l1lTkQ0ZFX6og4w72Ht7bbVnYOdNtlU7KDh3au%2Fxuiag8mlN%2FRNqRGlOiFaT7%2FOUSuWUsyZYtldtADgAr3Prf2XjciZ2Jh%2BAMiFs7mPM75rSuBZJFNeYibDwBpyPFHon5L599uKlAK7TeuDkTReB2TwcvWgWsLnUDLTZ1Vfis%2FgfKOYQwf7SwHqDIHioyPnoVQv%2B74IRVr1q7oie7B1gn%2FM3cQ9SAHDCa";
            pageDetail.LabelLinkToPolicyUrl = request.PageDetail.PolicyUrl;
            pageDetail.LabelLinkToPolicy = request.PageDetail.PolicyUrlText;
            pageDetail.RedirectUrl = request.PageDetail.RedirectUrl;
            pageDetail.HeaderLabelThankPage = request.PageDetail.SuccessHeaderText;
            pageDetail.ShortDescriptionThankPage = request.PageDetail.SuccessDescriptionText;
            pageDetail.ButtonThankpage = request.PageDetail.SuccessButtonText;

            //todo:change affter identity server
            pageDetail.LanguageCulture = request.Language;
            pageDetail.Status = Status.Active.ToString();
            pageDetail.Version += pageDetail.Version;
            pageDetail.CompanyId = request.authentication.CompanyID;
            pageDetail.UpdateBy = request.authentication.UserID;
            pageDetail.UpdateDate = DateTime.Now;
            _context.DbSetConsentPage.Update(pageDetail);

            #endregion

            await _context.SaveChangesAsync(cancellationToken);

            return entity.CollectionPointId;
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }
       
    }
}