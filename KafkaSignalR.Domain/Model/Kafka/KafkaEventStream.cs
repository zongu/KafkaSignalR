
namespace KafkaSignalR.Domain.Model.Kafka
{
    public class KafkaEventStream : EventStream
    {
        public KafkaEventStream(string type, string data, long utcTimeStamp)
        {
            Type = type;
            Data = data;
            UtcTimeStamp = utcTimeStamp;
        }
    }

    public interface IkafkaPubSubHandler : IPubSubHandler<KafkaEventStream>
    {

    }
}
