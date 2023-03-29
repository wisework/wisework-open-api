using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Domain.Exceptions;
public class UnsupportedVersionException : Exception
{ //todo:change คำที่สื้อว่า version ไม่ตรง update ไม่ได้

    public UnsupportedVersionException(int id)
        : base($"Version of id \"{id}\" is unsupported.")
    {
    }
}