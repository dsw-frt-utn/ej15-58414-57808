namespace Dsw2026Ej15.Domain.Exceptions;

public class ValidationException : Exception
{
    public  ValidationException(string msg) : base(msg)
    {
    }
}