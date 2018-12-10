
namespace KafkaSignalR.Domain.Model.Kafka
{
    public abstract class EventStream
    {
        public long UtcTimeStamp { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }
}
