using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public partial class Consent_SectionInfo:Base
{
    public int SectionInfoId { get; set; }
    public int CompanyId { get; set; }
    public string Status { get; set; } = null!;
    public string? Code { get; set; }
    public string Description { get; set; } = null!;
}
