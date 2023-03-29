using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_CollectionPointCustomFieldConfig
{
    public int CollectionPointCustomFieldConfigId { get; set; }
    public string? CollectionPointGuid { get; set; }
    public int? Sequence { get; set; }
    public int? CollectionPointCustomFieldId { get; set; }
    public bool? Required { get; set; }
}
