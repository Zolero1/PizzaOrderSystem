using DataContracts.Messages.Base;
using Infrastructure.Messaging;

namespace Infrastructure.Messaging.RabbitMq;

public class RabbitMqMessagingFactory
{
    public static async Task<RabbitMqReciver<T>> CreateReceiverAsync<T>(
        string exchange,
        CloudEventMessageRecived<T> onMessageReceived,
        string hostname = "localhost"
        ) where T : AMessage, new()
    {
        var receiver = new RabbitMqReciver<T>(exchange, hostname);
        receiver.OnMessageReceived += onMessageReceived;
        await receiver.ConnectAsync();
        return receiver;
    }

    public static async Task<RabbitMqSender> CreateSenderAsync(
        string exchange,
        string hostname = "localhost")
    {
        var sender = new RabbitMqSender(exchange, hostname);
        await sender.ConnectAsync();
        return sender;
    }
}