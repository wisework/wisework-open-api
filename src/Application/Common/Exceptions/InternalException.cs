using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WW.Application.Common.Exceptions;


public class InternalException : Exception
{
    public InternalException()
        : base()
    {
    }

    public InternalException(string message)
        : base(message)
    {
    }

    public InternalException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public InternalException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
