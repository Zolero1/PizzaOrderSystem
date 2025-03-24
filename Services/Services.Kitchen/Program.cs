using Amqp.Handler;
using DataContracts.DataTransferObjects;
using DataContracts.Messages;
using DataContracts.Messages.ServiceMessages;
using Infrastructure.Messaging;
using Infrastructure.Messaging.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("userClient", c =>
{
    c.BaseAddress = new Uri("http://localhost:5093/");
});

builder.Services.AddSingleton<IMessageSender>(_ =>
    RabbitMqMessagingFactory.CreateSenderAsync(Constants.ExchangeName).GetAwaiter().GetResult());

var app = builder.Build();

var scope = app.Services.CreateScope();
var clientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
IMessageSender sender = scope.ServiceProvider.GetRequiredService<IMessageSender>();
await RabbitMqMessagingFactory.CreateReceiverAsync<PaymentReceived>(Constants.ExchangeName,
    async (@event, message) =>
    {
        using var httpClient = clientFactory.CreateClient("userClient");
        var orderItems = await httpClient.GetFromJsonAsync<OrderItemsResponseDto>(
            $"order-items/{message.OrderId}");
        if (orderItems != null)
            throw new Exception("could not get items to cook");

        await Task.Delay(5_000);

        await sender.SendMessageAsync(new OrderPrepared(message));
    });

app.Run();