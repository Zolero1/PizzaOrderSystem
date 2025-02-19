using DataContracts.Messages.Base;
using Infrastructure.Messaging;
using RabbitMQ.Client;

namespace Infrastucture.Messaging.RabbitMq;

public class RabbitMqSender(string exchange, string hostname = "localhost") : IMessageSender 
{
    private IConnection? _connection;
    private IChannel? _channel;

    public async Task Connect()
    {
        var factory = new ConnectionFactory(){HostName = hostname};
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic);
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public Task SendMessageAsync(AMessage message)
    {
        throw new NotImplementedException();
    }
}