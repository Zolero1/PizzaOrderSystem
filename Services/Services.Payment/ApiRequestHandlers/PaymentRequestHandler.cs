using DataContracts.DataTransferObjects;
using DataContracts.Messages.ServiceMessages;
using Infrastructur.Messaging.Outbox.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Payment.Db;

namespace Services.Payment.ApiRequestHandlers;

public static class PaymentRequestHandler {
    public static async Task<PaymentResponseDto> HandlePayment(
        [FromBody] PaymentRequestDto dto,
        [FromServices] PaymentDbContext dbContext) {
        var pendingPayment = await dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == dto.OrderId);

        if (pendingPayment is null) {
            throw new Exception("Corresponding payment not found in DB!");
        }

        if (pendingPayment.PaymentAmount < dto.Amount) {
            throw new Exception("Gib ma mehr göd!!!");
        }

        var payedAt = DateTimeOffset.UtcNow;
        pendingPayment.PayedAt = payedAt;

        dbContext.Outbox.Add(OutboxMessage.FromMessage(new PaymentReceived() {
            OrderId = dto.OrderId
        }));
        
        await dbContext.SaveChangesAsync();

        return new PaymentResponseDto() {
            OrderId = dto.OrderId,
            PayedAt = payedAt
        };
    }
}