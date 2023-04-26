using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public  class Consent_CollectionPointCustomField: Base 
{
    public int CollectionPointCustomFieldId { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? Owner { get; set; }
    public int? CompanyId { get; set; }
    public string? Status { get; set; }
    public string? Placeholder { get; set; }
    public int? LengthLimit { get; set; }
    public int? MaxLines { get; set; }
    public int? MinLines { get; set; }
}