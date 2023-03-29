using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WW.Domain.Entities;
public class Consent_CollectionPoint :Base
{
    public int CollectionPointId { get; set; }
    public int? CompanyId { get; set; }
    public string? Status { get; set; }
    public string? CollectionPoint { get; set; }
    public int WebsiteId { get; set; }
    public string? Description { get; set; }
    public string? Script { get; set; }
    public string? Guid { get; set; }
    public string? KeepAliveData { get; set; }
    public bool? ActiveConsentCardNumber { get; set; }
    public bool? ActiveConsentCardNumberPk { get; set; }
    public bool? ActiveConsentCardNumberRequired { get; set; }
    public bool? ActiveConsentName { get; set; }
    public bool? ActiveConsentNamePk { get; set; }
    public bool? ActiveConsentNameRequired { get; set; }
    public bool? ActiveConsentEmail { get; set; }
    public bool? ActiveConsentEmailPk { get; set; }
    public bool? ActiveConsentEmailRequired { get; set; }
    public bool? ActiveConsentTel { get; set; }
    public bool? ActiveConsentTelPk { get; set; }
    public bool? ActiveConsentTelRequired { get; set; }
    public bool? ActiveConsentUid { get; set; }
    public bool? ActiveConsentUidpk { get; set; }
    public bool? ActiveConsentUidrequired { get; set; }
    public string? RedirectUrl { get; set; }
    //public string Language { get; set; }
   // public int Id { get; set; }
    //public List<Consent_Purpose> PurposesList { get; set; } = new List<Consent_Purpose>();
    // public List<Consent_CollectionPointCustomField> CustomFieldsList { get; set; } = new List<Consent_CollectionPointCustomField>();
}
