using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class ProgramDescription
{
    public int ProgramDescriptionID { get; set; }
    public int ProgramID { get; set; }
    public string Description { get; set; } = null!;
    public string LanguageCulture { get; set; } = null!;
}
