using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class ProgramAction : Base
{
    public int ProgramActionID { get; set; }
    public string Status { get; set; } = null!;
    public int ProgramID { get; set; }
    public int ActionID { get; set; }
}
