using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Common;
public class GeneralConsentFilterKey
{
    public string FullName { get; set; }

    public string PhoneNumber { get; set; }

    public string IdCardNumber { get; set; }

    public int? Uid { get; set; }
    public string Email { get; set; }
    public System.DateTimeOffset? StartDate { get; set; }
    public System.DateTimeOffset? EndDate { get; set; }
}
