using DataContracts.DataTransferObjects;
using DataContracts.Messages.ServiceMessages;
using Infrastructur.Messaging.Outbox.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.User.Db;
using Services.User.Domain;

namespace Services.User.ApiRequestHandler;

public static class UserRequestHandler {
    public static async Task<OrderResponseDto> HandleOrderRequest(
        [FromBody] OrderRequestDto dto,
        [FromServices] UserDbContext dbContext) {
        var orderId = Ulid.NewUlid().ToGuid();

        dbContext.OrderRequests.Add(new OrderRequest() {
            Id = orderId,
            CustomerName = dto.CustomerName,
            Address = dto.Address,
            Items = dto.Items.Select(i => new OrderRequestItem() {
                Article = i.Article,
                Price = i.Price
            }).ToList()
        });

        dbContext.Outbox.Add(OutboxMessage.FromMessage(
            new OrderReceived() {
                OrderId = orderId,
                TotalValue = dto.Items.Sum(i => i.Price)
            }
        ));

        await dbContext.SaveChangesAsync();

        return new OrderResponseDto() {
            OrderId = orderId
        };
    }

    public static async Task<OrderItemsResponseDto> HandleOrderItemsRequest(
        [FromRoute] Guid orderId,
        [FromServices] UserDbContext dbContext) {
        var order = await dbContext.OrderRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == orderId);

        if (order is null) {
            throw new Exception("Order not found");
        }

        return new OrderItemsResponseDto() {
            Items = order.Items.Select(i => new ArticleDto() {
                Name = i.Article
            }).ToList()
        };
    }
}