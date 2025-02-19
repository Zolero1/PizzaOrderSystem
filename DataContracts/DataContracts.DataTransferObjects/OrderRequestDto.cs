namespace DataContracts.DataTransferObjects;

public class OrderRequestDto
{
    public string CustomerName { get; set; }
    public string Address { get; set; }

    public List<OrderItemDto> Items { get; set; } = [];
}