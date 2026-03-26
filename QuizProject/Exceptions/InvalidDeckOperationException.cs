using System;

// You created and used your own exception types (1 point)
public class InvalidDeckOperationException : Exception
{
    public InvalidDeckOperationException()
    {
    }

    public InvalidDeckOperationException(string message)
        : base(message)
    {
    }

    public InvalidDeckOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
