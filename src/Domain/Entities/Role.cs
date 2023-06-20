using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Role : Base
{
    public long RoleID { get; set; }
    public string Status { get; set; } = null!;
    public long CompanyID { get; set; }
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
}
