using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WW.Application.Common.Models;

namespace WW.Application.GeneralConsents.Queries;
public class GeneralConsentDisplay : PrimaryKeyConfiguration
{
    public int ConsentID { get; set; } = 0;
    public int CollectionPointID { get; set; } = 0;
    public string Code { get; set; } = string.Empty;

    public int TotalTransactions { get; set; } = 0;
    public string NameSurname { get; set; } = string.Empty;
    public string CollectionPointGUID { get; set; } = string.Empty;
    public DateTimeOffset ConsentDateTime { get; set; } = DateTimeOffset.MinValue;

    public string ConsentDateTimeDisplay { get; set; } = string.Empty;
    public int WebsiteID { get; set; } = 0;
    public int CollectionPointVersion { get; set; } = 0;
    public string WebsiteCode { get; set; } = string.Empty;
    public string WebsiteDescription { get; set; } = string.Empty;
    public int PurposeID { get; set; } = 0;
    public string PurposeCode { get; set; } = string.Empty;
    public int Priority { get; set; } = 0;
    public string KeepAliveData { get; set; } = string.Empty;
    public int PurposeCategoryID { get; set; } = 0;
    public string WarningDescription { get; set; } = string.Empty;
    public string LinkMoreDetail { get; set; } = string.Empty;
    public string PurposeType { get; set; } = string.Empty;
    public DateTimeOffset PurposeCreateDate { get; set; } = DateTimeOffset.MinValue;

    public string PurposeDescription { get; set; } = string.Empty;
    public DateTimeOffset ConsentConsentItemExpired { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset Expired { get; set; } = DateTimeOffset.MinValue;
    public string PurposeGUID { get; set; } = string.Empty;
    public string PurposeList { get; set; } = string.Empty;
    public int ConsentActive { get; set; } = 0;
    public string ConsentActiveDisplay { get; set; } = string.Empty;
    public string FromBrowser { get; set; } = string.Empty;
    public string FromWebsite { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public string EventCode { get; set; } = string.Empty;
    public int Uid { get; set; } = 0;
    public string CollectionPointName { get; set; } = string.Empty;

    public int totalCount { get; set; } = 0;

    public int CompanyID { get; set; } = 0;
    public string CompanyName { get; set; } = string.Empty;
    public int Version { get; set; } = 0;
    public string Status { get; set; } = string.Empty;
    public string StatusDisplay { get; set; } = string.Empty;

    public int CreateBy { get; set; } = 0;
    public string CreateByDisplay { get; set; } = string.Empty;
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.MinValue;
    public string CreateDateDisplay { get; set; } = string.Empty;

    public int UpdateBy { get; set; } = 0;
    public string UpdateByDisplay { get; set; } = string.Empty;
    public DateTimeOffset UpdateDate { get; set; } = DateTimeOffset.MinValue;
    public string UpdateDateDisplay { get; set; } = string.Empty;
    public string VerifyType { get; set; } = string.Empty;

    public int Row { get; set; } = 0;
    public string TextMoreDetail { get; set; } = string.Empty;

    public bool ActiveConsentDisplay
    {
        get
        {
            if (ConsentActive == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsStatus
    {
        get
        {
            if (Status == "Active")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public string CreateSystemDisplay
    {
        get
        {
            string createSystem = string.Format("{0} {1}", CreateByDisplay, CreateDateDisplay);
            return createSystem;
        }
    }

    public string UpdateSystemDisplay
    {
        get
        {
            string updateSystem = string.Format("{0} {1}", UpdateByDisplay, UpdateDateDisplay);
            return updateSystem;
        }
    }
    public List<CollectionPointCustomField> CollectionPointCustomField
    {
        get; set;
    }
    public string AgeRange { get; set; } = string.Empty;
}
