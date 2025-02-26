using DataContracts.Messages.Base;

namespace DataContracts.Messages.ServiceMessages;

public class OrderReceived : AOrderIdMessage
{
    private const string DefaultUserName = "https://user.pizza-order-system.com";
    public OrderReceived()
    {
        SourceUri = new Uri(DefaultUserName);
    }

    public OrderReceived(AOrderIdMessage other) : base(other)
    {
        SourceUri = new Uri(DefaultUserName);
    }
    
    public decimal TotalValue { get; set; }
    
    public override string MessageType() => Constants.OrderReceivedV1;
}