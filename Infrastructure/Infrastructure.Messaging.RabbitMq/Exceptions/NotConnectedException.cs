namespace Infrastructure.Messaging.RabbitMq.Exceptions;

public class NotConnectedException : Exception
{
    public NotConnectedException()
    {
        
    }

    public NotConnectedException(string message) : base(message)
    {
        
    }

    public NotConnectedException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}