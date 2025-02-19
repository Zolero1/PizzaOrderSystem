using System.Net.Mime;
using System.Text.Json;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;

namespace DataContracts.Messages.Base;

public abstract class AMessage
{
    public Uri SourceUri { get; init; }

    public abstract string MessageType();

    private CloudEvent ToCloudEvent()
    {
        return new CloudEvent()
        {
            Id = Guid.NewGuid().ToString(),
            Type = MessageType(),
            Source = SourceUri,
            Time = DateTimeOffset.UtcNow,
            DataContentType = "application/json",
            Data = this
        };
    }

    public byte[] Serialize()
        {
            var ce = ToCloudEvent();
            var encoded= new JsonEventFormatter().EncodeStructuredModeMessage(ce, out var contentType);
            return encoded.ToArray();
        }

        public JsonElement SerializeToJson()
        {
            var ce = ToCloudEvent();
            return new JsonEventFormatter().ConvertToJsonElement(ce);
        }
    
        public static CloudEvent Deserialize(byte[] bytes)
        {
            var cloudEvent = new JsonEventFormatter().DecodeStructuredModeMessage(new ReadOnlyMemory<byte>(bytes),
                new ContentType("application/json"), null);
            return cloudEvent;
        }

        public static CloudEvent Deserialize(JsonElement jsonElement)
        {
            var cloudEvent = new JsonEventFormatter().ConvertFromJsonElement(jsonElement, null);
            return cloudEvent;
        }
    
        public static (CloudEvent cloudEvent, T?) Deserialize<T>(byte[] bytes)
            where T : AMessage
        {
            var cloudEvent = Deserialize(bytes);
            return (cloudEvent, ((JsonElement?)cloudEvent.Data)?.Deserialize<T>());
        }
    
        public static (CloudEvent cloudEvent, T?) Deserialize<T>(JsonElement jsonElement)
            where T : AMessage
        {
            var cloudEvent = Deserialize(jsonElement);
            return (cloudEvent, ((JsonElement?)cloudEvent.Data)?.Deserialize<T>());
        }

        public static (CloudEvent cloudEvent, object?) Deserialize(JsonElement jsonElement, Type returnType)
        {
            var cloudEvent = Deserialize(jsonElement);
            return (cloudEvent, ((JsonElement?)cloudEvent.Data)?.Deserialize(returnType));
        }
}