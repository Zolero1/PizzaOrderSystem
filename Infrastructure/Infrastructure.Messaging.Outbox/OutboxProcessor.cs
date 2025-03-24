using System.Text.Json;
using Amqp.Listener;
using DataContracts.Messages;
using DataContracts.Messages.Base;
using DataContracts.Messages.ServiceMessages;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructur.Messaging.Outbox;

public class OutboxProcessor {
    private static readonly Dictionary<string, Type> MessageTypes = new() {
        { Constants.OrderReceivedV1, typeof(OrderReceived) }
    };

    public int ProcessingDelayMs { get; set; } = 2_000;

    public async Task Process(Func<OutboxDbContext> dbContextResolver,
        IMessageSender messageSender,
        CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            await Task.Delay(ProcessingDelayMs, stoppingToken);

            await using var dbContext = dbContextResolver();

            var outboxMessages = await dbContext.Outbox
                .Where(o => o.SentAt == null)
                .OrderBy(o => o.Id)
                .Take(10)
                .ToListAsync(stoppingToken);

            foreach (var message in outboxMessages) {
                using JsonDocument doc = JsonDocument.Parse(message.Message);
                JsonElement root = doc.RootElement;

                if (MessageTypes.TryGetValue(message.MessageType, out var messageType)) {
                    var (_, msg) = AMessage.Deserialize(root, messageType);
                    if (msg is AMessage messageToSend) {
                        await messageSender.SendMessageAsync(messageToSend);
                    }

                    message.SentAt = DateTimeOffset.UtcNow;
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                else {
                    throw new UnsupportedOutboxMessageException($"Message type: {message.MessageType}");
                }
            }
        }
    }
}