using DataContracts.Messages;
using Infrastructure.Messaging;
using Infrastructure.Messaging.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Services.User.ApiRequestHandler;
using Services.User.Db;
using Services.User.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("UserDbConnection")
                       ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContextFactory<UserDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddSingleton<IMessageSender>(_ =>
    RabbitMqMessagingFactory.CreateSenderAsync(Constants.ExchangeName).GetAwaiter().GetResult());

builder.Services.AddHostedService<OutboxProcessorBgService>();

var app = builder.Build();

var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetService<UserDbContext>();
var connected = false;

while (!connected) {
    if (!dbContext!.Database.CanConnect()) {
        await Task.Delay(1_000);
        continue;
    }

    dbContext.Database.Migrate();
    connected = true;
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapPost("/order", UserRequestHandler.HandleOrderRequest)
    .WithName("Place Order")
    .WithOpenApi();

app.MapGet("/order-items/{orderId:guid}", UserRequestHandler.HandleOrderItemsRequest)
    .WithName("Order Items")
    .WithOpenApi();

app.Run();