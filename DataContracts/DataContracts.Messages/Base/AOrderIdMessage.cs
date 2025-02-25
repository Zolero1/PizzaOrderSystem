namespace DataContracts.Messages.Base;

public abstract class AOrderIdMessage : AMessage
{
    protected AOrderIdMessage() { }

    protected AOrderIdMessage(AOrderIdMessage other)
    {
        OrderId = other.OrderId;
    }

    public Guid OrderId { get; set; }
}