using DataContracts.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Services.User.ApiRequestHandler;

public static class UserRequestHandler
{
    public static async Task<OrderResponseDto> HandleOrderRequest([FromBody] OrderRequestDto dto)
    {
        var orderId = Ulid.NewUlid().ToGuid();
        // TODO: Store order in database
        // TODO: Send OrderReceived Message
    }
}