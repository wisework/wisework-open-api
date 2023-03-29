using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class Base
{
    public int? CreateBy { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public int? UpdateBy { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    public int? Version { get; set; }
}
