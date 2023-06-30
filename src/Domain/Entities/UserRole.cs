using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class UserRole : Base
{
    public long UserRoleID { get; set; }
    public int CompanyID { get; set; }
    public string? Status { get; set; }
    public long RoleID { get; set; }
    public long UserID { get; set; }
}
