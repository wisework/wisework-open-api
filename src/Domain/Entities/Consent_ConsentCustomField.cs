using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_ConsentCustomField
{
    public int ConsentCustomFieldId { get; set; }
    public int? CollectionPointCustomFieldConfigID { get; set; }
    public string? Value { get; set; }
    public int? ConsentId { get; set; }
}
