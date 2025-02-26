namespace DataContracts.DataTransferObjects;

public class PaymentRequestDto {
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}