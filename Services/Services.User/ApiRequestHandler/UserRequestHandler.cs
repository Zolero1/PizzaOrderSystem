using DataContracts.DataTransferObjects;
using DataContracts.Messages.ServiceMessages;
using Infrastructur.Messaging.Outbox.Domain;
using Microsoft.AspNetCore.Mvc;
using Services.User.Db;
using Services.User.Domain;

namespace Services.User.ApiRequestHandler;

public static class UserRequestHandler
{
    public static async Task<OrderResponseDto> HandleOrderRequest(
        [FromBody] OrderRequestDto dto,
        [FromServices] UserDbContext dbContext
    )
    {
        var orderId = Ulid.NewUlid().ToGuid();

        dbContext.OrderRequests.Add(new OrderRequest()
        {
            Id = orderId,
            CustomerName = dto.CustomerName,
            Address = dto.Address,
            Items = dto.Items.Select(i => new OrderRequestItem
            {
                Article = i.Article,
                Price = i.Price
            }).ToList(),
        });
        dbContext.Outbox.Add(OutboxMessage.FromMessage(
            new OrderRecived()
            {
                OrderId = orderId,
                TotalValue = dto.Items.Sum(i=>i.Price)
            }));
        await dbContext.SaveChangesAsync();

        return new OrderResponseDto()
        {
            OrderId = orderId
        };
    }
}