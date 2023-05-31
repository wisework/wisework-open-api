using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Consent_PurposeCategory : Base
{
    public int ID { get; set; }
    public int PurposeCategoryID { get; set; }
    public int CompanyID { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Language { get; set; }
    public string Status { get; set; }
}
