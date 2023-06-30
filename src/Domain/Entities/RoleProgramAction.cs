using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class RoleProgramAction
{
    public long RoleProgramActionID { get; set; }
    public long RoleID { get; set; }
    public int ProgramActionID { get; set; }
}
