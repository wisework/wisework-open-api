using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Program : Base
{
    public int ProgramID { get; set; }
    public string Status { get; set; } = null!;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int ParentID { get; set; }
    public string? Action { get; set; }
    public string? Icon { get; set; }
    public int Badge { get; set; }
    public int? expanded { get; set; }
    public int Priority { get; set; }
}
