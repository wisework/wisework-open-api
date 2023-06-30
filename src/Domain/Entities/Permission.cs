using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Permission
{
    public string Action { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ParentID { get; set; } = 0;
    public int Priority { get; set; } = 0;
    public int ProgramGroupID { get; set; } = 0;
    public int ProgramID { get; set; } = 0;
    public bool IsExpanded { get; set; } = false;
    public string Icon { get; set; } = string.Empty;

    public List<Permission> items = new List<Permission>();

}
