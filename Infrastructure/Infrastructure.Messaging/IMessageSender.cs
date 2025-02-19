using DataContracts.Messages.Base;

namespace Infrastructure.Messaging;

public interface IMessageSender : IDisposable, IAsyncDisposable
{
    Task SendMessageAsync(AMessage message);
}