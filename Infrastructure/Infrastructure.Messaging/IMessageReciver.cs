using DataContracts.Messages.Base;

namespace Infrastructure.Messaging;

public interface IMessageReciver<out T> : IDisposable, IAsyncDisposable where T : AMessage, new()
{
    event CloudEventMessageRecived<T>? OnMessageReceived;
}