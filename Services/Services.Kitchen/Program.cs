using DataContracts.Messages;
using DataContracts.Messages.ServiceMessages;
using Infrastructure.Messaging;
using Infrastructure.Messaging.RabbitMq;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("userClient", c => {
    c.BaseAddress = new Uri("http://localhost:5073/");
});

builder.Services.AddSingleton<IMessageSender>(_ =>
    RabbitMqMessagingFactory.CreateSenderAsync(Constants.ExchangeName).GetAwaiter().GetResult());


var app = builder.Build();


await RabbitMqMessagingFactory.CreateReceiverAsync<PaymentReceived>(Constants.ExchangeName, async (@event, message) => {

});

app.Run();