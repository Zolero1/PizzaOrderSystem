namespace Infrastructure.Messaging;

public class UnsupportedOutboxMessageException : Exception
{
    public UnsupportedOutboxMessageException()
    {
        
    }

    public UnsupportedOutboxMessageException(string message) : base(message)
    {
        
    }

    public UnsupportedOutboxMessageException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}