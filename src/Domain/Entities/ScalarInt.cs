using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Entities;
public class ScalarInt
{
    public int Value { get; set; }

    public override string ToString()
    {
        return Value.ToString();
    }
}
