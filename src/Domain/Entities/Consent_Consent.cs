using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wisework.ConsentManagementSystem.Api;

namespace WW.Domain.Entities;
public class Consent_Consent : Base
{
    public int ConsentId { get; set; }
    public int? CompanyId { get; set; }
    public int CollectionPointId { get; set; }
    public DateTimeOffset? ConsentDatetime { get; set; }
    public int? WebsiteId { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    //public string? Tel { get; set; }
    public string? FromBrowser { get; set; }
    public string? FromWebsite { get; set; }
    public string? ConsentSignature { get; set; }
    public string? VerifyType { get; set; }
    public int? TotalTransactions { get; set; }
    public int? New { get; set; }
    public string? CardNumber { get; set; }
    public string? Remark { get; set; }
    public string? EventCode { get; set; }
    public DateTimeOffset? Expired { get; set; }
    public bool? HasNotificationRenew { get; set; }
    public int? Uid { get; set; }
    public string? AgeRange { get; set; }
    public string? Status { get; set; }
    //public Guid? CollectionPointGuid { get; set; }
    public int? WebSiteId { get; set; }
    //public int? CollectionPointVersion { get; set; }
    //public GeneralConsentPurpose PurposeList { get; set; }
    public string? PhoneNumber { get; set; }
    public string? IdCardNumber { get; set; }
    //public int? TotalCount { get; set; }
    //public string? CompanyName { get; set; }
}
