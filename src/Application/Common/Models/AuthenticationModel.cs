using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Application.Common.Models;
public class AuthenticationModel
{
    public string TokenID { get; set; } = string.Empty;
    public int UserID { get; set; } = 0;
    public int CompanyID { get; set; } = 0;
    public DateTime TokenDate { get; set; } = DateTime.MinValue;
    public string VisitorId { get; set; } = "";
}
