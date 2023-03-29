using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_ConsentItem
{
    public int ConsentItemId { get; set; }
    public int? CompanyId { get; set; }
    public int ConsentId { get; set; }
    public int? CollectionPointItemId { get; set; }
    public int? ConsentActive { get; set; }
    public DateTimeOffset? Expired { get; set; }
}
