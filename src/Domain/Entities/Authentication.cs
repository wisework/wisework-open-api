using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Authentication
{
    public string TokenID { get; set; } = string.Empty;
    public int UserID { get; set; } = 0;
    public int CompanyID { get; set; } = 0;
    public int DefualtBranch { get; set; } = 0;
    public int LanguageID { get; set; } = 0;
    public string Language { get; set; } = string.Empty;
    public DateTime TokenDate { get; set; } = DateTime.MinValue;
    public string BrowserAgent { get; set; } = "";
    public string PlatformType { get; set; } = "";

    public string OsName { get; set; } = "";
    public string Provide { get; set; } = string.Empty;
    public string access_token { get; set; } = string.Empty;
    public int Code { get; set; } = 0;

    public string VisitorId { get; set; } = "";
}