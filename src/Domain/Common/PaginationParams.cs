using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Common;
public class PaginationParams
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}
