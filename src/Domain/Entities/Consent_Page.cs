using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_Page :Base
{
    public int PageId { get; set; }
    public int? CompanyId { get; set; }
    public string? Status { get; set; }
    public int CollectionPointId { get; set; }
    public int? HeaderLogo { get; set; }
    public string? HeaderLabel { get; set; }
    public string? HeaderBgcolor { get; set; }
    public int? HeaderBgimage { get; set; }
    public string? HeaderFontColor { get; set; }
    public string? BodyBgcolor { get; set; }
    public int? BodyBgimage { get; set; }
    public string? BodyTopDescription { get; set; }
    public string? BodyTopDerscriptionFontColor { get; set; }
    public string? BodyBottomDescription { get; set; }
    public string? BodyBottomDescriptionFontColor { get; set; }
    public string? OkbuttonFontColor { get; set; }
    public string? OkbuttonBgcolor { get; set; }
    public string? CancelButtonFontColor { get; set; }
    public string? CancelButtonBgcolor { get; set; }
    public string? LabelCheckBoxAccept { get; set; }
    public string? LabelCheckBoxAcceptFontColor { get; set; }
    public string? LabelLinkToPolicy { get; set; }
    public string? LabelLinkToPolicyUrl { get; set; }
    public string? LabelLinkToPolicyFontColor { get; set; }
    public string? UrlconsentPage { get; set; }
    public string? LabelActionOk { get; set; }
    public string? LabelActionCancel { get; set; }
    public string? LabelPurposeActionAgree { get; set; }
    public string? LabelPurposeActionNotAgree { get; set; }
    public string? HeaderLabelThankPage { get; set; }
    public string? ButtonThankpage { get; set; }
    public string? LanguageCulture { get; set; }
    public string? ShortDescriptionThankPage { get; set; }
    public string? RedirectUrl { get; set; }
    public int ThemeId { get; set; }
}
