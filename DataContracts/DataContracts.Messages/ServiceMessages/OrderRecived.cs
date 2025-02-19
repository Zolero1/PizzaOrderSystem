using DataContracts.Messages.Base;

namespace DataContracts.Messages.ServiceMessages;

public class OrderRecived : AOrderIdMessage
{
    private const string DefaultUserName = "https://user.pizza-order-system.com";
    public OrderRecived()
    {
        SourceUri = new Uri(DefaultUserName);
    }

    public OrderRecived(AOrderIdMessage other) : base(other)
    {
        SourceUri = new Uri(DefaultUserName);
    }
    
    public decimal TotalValue { get; set; }
    
    public override string MessageType() => Constants.OrderRecievedV1;
}