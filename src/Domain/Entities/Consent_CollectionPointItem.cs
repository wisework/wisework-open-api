using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_CollectionPointItem : Base
{
    public int CollectionPointItemId { get; set; }
    public int? CompanyId { get; set; }
    public int CollectionPointId { get; set; }
    public int PurposeId { get; set; }
    public int? Priority { get; set; }
    public string? Status { get; set; }
    public int? SectionInfoId { get; set; }
}
