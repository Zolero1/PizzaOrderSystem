using Infrastructure.Messaging;
using Infrastucture.Messaging.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Services.User.ApiRequestHandler;
using Services.User.Db;
using Constants = RabbitMQ.Client.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("UserDbConnection")?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContextFactory<UserDbContext>(options => 
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<IMessageSender>(_ => RabbitMqMessagingFactory.CreateSenderAsync(DataContracts.Messages.Constants.ExchangeName).GetAwaiter().GetResult());

var app = builder.Build();

var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetService<UserDbContext>();

var connected = false;

while (!connected)
{
    if (!dbContext.Database.CanConnect())
    {
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

app.Run();

