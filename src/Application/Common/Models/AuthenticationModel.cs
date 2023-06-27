using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSwag.Annotations;

namespace WW.Application.Common.Models;
public class AuthenticationModel
{
    [OpenApiIgnoreAttribute]
    public string TokenID { get; set; } = string.Empty;
    [OpenApiIgnoreAttribute]
    public int UserID { get; set; } = 0;
    [OpenApiIgnoreAttribute]
    public int CompanyID { get; set; } = 0;
    [OpenApiIgnoreAttribute]
    public DateTime TokenDate { get; set; } = DateTime.MinValue;
    [OpenApiIgnoreAttribute]
    public string VisitorId { get; set; } = "";
}
