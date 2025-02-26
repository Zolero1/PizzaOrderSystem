using DataContracts.Messages.Base;

namespace DataContracts.Messages.ServiceMessages;

public class PaymentReceived : AOrderIdMessage {
    private const string DefaultUserName = "https://payment.pizza-order-system.com";

    public PaymentReceived() {
        SourceUri = new Uri(DefaultUserName);
    }

    public PaymentReceived(AOrderIdMessage other) : base(other) {
        SourceUri = new Uri(DefaultUserName);
    }

    public override string MessageType() => Constants.PaymentReceivedV1;
}