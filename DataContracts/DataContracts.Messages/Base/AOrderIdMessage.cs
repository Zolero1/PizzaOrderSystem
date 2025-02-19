namespace DataContracts.Messages.Base;

public abstract class AOrderIdMessage : AMessage
{
    protected AOrderIdMessage() { }

    protected AOrderIdMessage(AOrderIdMessage other)
    {
        OderId = other.OderId;
    }

    public Guid OderId { get; set; }
}