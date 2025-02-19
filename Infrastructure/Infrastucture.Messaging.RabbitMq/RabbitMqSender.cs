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
        _connection?.Dispose();
        _channel?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if(_connection != null) await _connection.DisposeAsync();
        if(_channel != null) await _channel.DisposeAsync();
    }

    public async Task SendMessageAsync(AMessage message)
    {
        if (_channel is null)
        {
            throw new NotSupportedException("Channel is not configured");
        }

        await _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: message.MessageType(),
            mandatory: false,
            basicProperties: new BasicProperties(),
            body: message.Serialize()
        );
    }
}