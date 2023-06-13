using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class CompanyUser : Base
{
    public long CompanyUserID { get; set; }
    public string Status { get; set; }
    public int UserID { get; set; }
    public long CompanyID { get; set; }
    public int DefaultBranch { get; set; }
}
