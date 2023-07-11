using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_Purpose : Base
{
    public int PurposeId { get; set; }
    public int CompanyId { get; set; }
    public string? Status { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Guid { get; set; }
    public int PurposeType { get; set; }
    public string? TextMoreDetail { get; set; }
    public string? LinkMoreDetail { get; set; }
    public int PurposeCategoryId { get; set; }
    public string? WarningDescription { get; set; }
    public string? KeepAliveData { get; set; }
    public string? ExpiredDateTime { get; set; }
    public string? Language { get; set; }

}
