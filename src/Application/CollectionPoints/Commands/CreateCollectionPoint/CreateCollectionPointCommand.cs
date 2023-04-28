using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Wisework.ConsentManagementSystem.Api;
using WW.Application.Common.Interfaces;
using WW.Domain.Entities;
using WW.Domain.Enums;

namespace WW.Application.CollectionPoints.Commands.CreateCollectionPoint;

public record CreateCollectionPointCommand : IRequest<int>
{
    public string Code { get; init; }
    public List<CollectionPointConsentKeyIdentifier> ConsentKeyIdentifier { get; init; }
    public List<CollectionPointCustomFields> CustomFieldsList { get; init; }
    public string ExpirationPeriod { get; init; }
    public string Language { get; init; }
    public CollectionPointPageDetail PageDetail { get; init; }
    public List<CollectionPointPurpose> PurposesList { get; init; }
    public int WebsiteId { get; init; }

}

public class CreateTodoListCommandHandler : IRequestHandler<CreateCollectionPointCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCollectionPointCommand request, CancellationToken cancellationToken)
    {
        #region add collection point and PrimaryKey
        var entity = new Consent_CollectionPoint();
        entity.CollectionPoint = request.Code;
        entity.WebsiteId = request.WebsiteId;
        entity.KeepAliveData = request.ExpirationPeriod;
        //entity.Language = request.Language;
        //todo:change affter identity server
        entity.CreateBy = 1;
        entity.UpdateBy = 1;
        entity.UpdateDate = DateTime.Now;
        entity.CreateDate = DateTime.Now;
        //entity.CreatedBy = "1";

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
            purposeEntity.CreateBy = 1;
            purposeEntity.UpdateBy = 1;
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
        // id collecion point
        pageDetail.CollectionPointId = entity.CollectionPointId;
        //pageDetail.LabelCheckBoxAccept = request.PageDetail.AcceptCheckBoxLabel;
        //pageDetail.LabelCheckBoxAcceptFontColor = request.PageDetail.AcceptCheckBoxLabelFontColor;
        //pageDetail.BodyBgcolor = request.PageDetail.BodyBackgroundColor;
        //pageDetail.BodyBgimage = request.PageDetail.BodyBackgroundId;
        //pageDetail.BodyBottomDescription = request.PageDetail.BodyBottomDescription;
        //pageDetail.BodyBottomDescriptionFontColor = request.PageDetail.BodyBottomDescriptionFontColor;
        //pageDetail.BodyTopDescription = request.PageDetail.BodyTopDescription;
        //pageDetail.BodyTopDerscriptionFontColor = request.PageDetail.BodyTopDerscriptionFontColor;
        //pageDetail.CancelButtonBgcolor = request.PageDetail.CancelButtonBackgroundColor;
        //pageDetail.CancelButtonFontColor = request.PageDetail.CancelButtonFontColor;
        //pageDetail.LabelActionCancel = request.PageDetail.CancelButtonLabel;
        //pageDetail.LabelActionOk = request.PageDetail.ConfirmButtonLabel;
        //pageDetail.HeaderBgcolor = request.PageDetail.HeaderBackgroundColor;
        //pageDetail.HeaderBgimage = request.PageDetail.HeaderBackgroundId;
        //pageDetail.HeaderFontColor = request.PageDetail.HeaderFontColor;
        //pageDetail.HeaderLabel = request.PageDetail.HeaderLabel;
        //pageDetail.HeaderLogo = request.PageDetail.LogoId;
        //pageDetail.OkbuttonBgcolor = request.PageDetail.OkButtonBackgroundColor;
        //pageDetail.OkbuttonFontColor = request.PageDetail.OkButtonFontColor;
        //pageDetail.LabelLinkToPolicyUrl = request.PageDetail.PolicyUrl;
        //pageDetail.LabelLinkToPolicy = request.PageDetail.PolicyUrlLabel;
        //pageDetail.LabelPurposeActionAgree = request.PageDetail.PurposeAcceptLabel;
        //pageDetail.LabelLinkToPolicyFontColor = request.PageDetail.PolicyUrlLabelFontColor;
        //pageDetail.LabelPurposeActionNotAgree = request.PageDetail.PurposeRejectLabel;
        pageDetail.RedirectUrl = request.PageDetail.RedirectUrl;
        //pageDetail.HeaderLabelThankPage = request.PageDetail.SuccessHeaderLabel;
        //pageDetail.ShortDescriptionThankPage = request.PageDetail.SuccessDescription;
        //pageDetail.ButtonThankpage = request.PageDetail.SuccessButtonLabel;

        //todo:change affter identity server
        /*pageDetail.CreateBy = 1;
        pageDetail.UpdateBy = 1;
        pageDetail.UpdateDate = DateTime.Now;
        pageDetail.CreateDate = DateTime.Now;
        //test check error
        pageDetail.CreatedBy = "1";*/

        _context.DbSetConsentPage.Add(pageDetail);

        #endregion

        await _context.SaveChangesAsync(cancellationToken);
      
        return entity.CollectionPointId;
    }
}
