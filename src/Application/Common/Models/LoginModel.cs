using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Application.Common.Models;
public class LoginModel
{
    public int? CompanyId { get; set; }

    public string? Email { get; set; }

    public string? Guid { get; set; }

    public int? UserID { get; set; }

    public string? Username { get; set; }

    public int? Version { get; set; }
}

