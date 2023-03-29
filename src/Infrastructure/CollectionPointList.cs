using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Infrastructure;
 public partial class CollectionPointList
{
    public CollectionPointList()
    {
    }

    public int CollectionPointId { get; set; }
    public int? CompanyId { get; set; }
    public int? Version { get; set; }
    public string? Status { get; set; }
    public int? CreateBy { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    public int? UpdateBy { get; set; }
    public DateTimeOffset? UpdateDate { get; set; }
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
}
