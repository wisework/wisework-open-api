using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class TotalRow 
{
    public int TotalRowId { get; set; }
    public int? CompanyId { get; set; }
    public string? TableName { get; set; }
    public int? TotalCountRow { get; set; }
    public int? TotalCountGroup { get; set; }
    public int? CreateBy { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
}
