using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Position : Base
{
    public int PositionID { get; set; }
    public long CompanyID { get; set; }
    public string Status { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public int DepartmentID { get; set; }
}
