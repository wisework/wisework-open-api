using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class LanguageDisplay
{
    public int LanguageID { get; set; }
    public int? Version { get; set; }
    public string Status { get; set; } = string.Empty;
    public int CreateBy { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public int UpdateBy { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Name  { get; set; }
    public string? LanguageCulture { get; set; }
    public string? IconUrl { get; set; }
    public int CompanyID { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDefault { get; set; }

}
