using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_Consent
{
    public int ConsentId { get; set; }
    public int? CompanyId { get; set; }
    public int CollectionPointId { get; set; }
    public DateTimeOffset? ConsentDatetime { get; set; }
    public int? WebsiteId { get; set; }
    public string? Email { get; set; }
    public string? NameSurname { get; set; }
    public string? Tel { get; set; }
    public int? Createby { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
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
    public int? UpdateBy { get; set; }
    public DateTimeOffset? UpdateDate { get; set; }
}
