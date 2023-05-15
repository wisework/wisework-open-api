using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class ConsentWebsite
{
    public int WebsiteId { get; set; }
    public int CompanyId { get; set; }
    public int? Version { get; set; }
    public string Status { get; set; } = null!;
    public int CreateBy { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public int UpdateBy { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public string? Code { get; set; }
    public string Description { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Urlpolicy { get; set; } = null!;
    public string? Name { get; set; }
    public string? UrlHomePage { get; set; }
    public string? UrlPolicyPage { get; set; }
}
