using System.Reflection.Metadata.Ecma335;
using DataContracts.Messages.Base;
using Infrastructure.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastucture.Messaging.RabbitMq;

public class RabbitMqReciver<T>(string exchange, string hostname = "localhost") : IMessageReciver<T> where T : AMessage, new()
{
    private IConnection? _connection;
    private IChannel? _channel;
    private AsyncEventingBasicConsumer? _consumer;

    public event CloudEventMessageRecived<T>? OnMessageReceived;

    public async Task ConnectAsync()
    {
        var factory = new ConnectionFactory(){HostName = hostname};
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        
        await _channel.ExchangeDeclareAsync(exchange: exchange, ExchangeType.Topic);
        
        var startAssembly = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name ?? "Receiver";
        var messageType = new T().MessageType();
        var queueName = $"{startAssembly}_{messageType}";
        
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: exchange,
            routingKey: messageType,
            arguments: null);
        
        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.ReceivedAsync += ConsumerOnReceivedAsync;
        
        await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: _consumer);
    }

    private async Task ConsumerOnReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var (cloudEvent, message) = AMessage.Deserialize<T>(eventArgs.Body.ToArray());

        try
        {
            if (OnMessageReceived is not null && message is not null)
            {
                await OnMessageReceived(cloudEvent, message);
            }

            await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        }
        catch
        {
            // TODO reque limitieren mit BadLetterQueue
            await _channel.BasicNackAsync(eventArgs.DeliveryTag, false, false);
        }
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
}