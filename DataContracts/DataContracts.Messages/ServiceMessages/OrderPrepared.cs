using DataContracts.Messages.Base;

namespace DataContracts.Messages.ServiceMessages;

public class OrderPrepared : AOrderIdMessage
{
    private const string DefaultSource = "https://kitchen.pizza-order-system.com";
    
    public OrderPrepared()
    {
        SourceUri = new Uri(DefaultSource);
    }

    public OrderPrepared(AOrderIdMessage other) : base(other)
    {
        SourceUri = new Uri(DefaultSource);
    }
    
    public override string MessageType() => Constants.OrderPreparedV1;
}