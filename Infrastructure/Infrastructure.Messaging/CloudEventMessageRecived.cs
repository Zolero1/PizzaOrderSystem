using CloudNative.CloudEvents;
using DataContracts.Messages.Base;

namespace Infrastructure.Messaging;

public delegate Task CloudEventMessageRecived<in TMessage>(CloudEvent cloudEvent, TMessage message)
    where TMessage : AMessage;