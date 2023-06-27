namespace WW.Application.Common.Exceptions;
public class InternalServerException : Exception
{
    public InternalServerException() : base() { }

    public InternalServerException(string message) : base(message) { }
}
