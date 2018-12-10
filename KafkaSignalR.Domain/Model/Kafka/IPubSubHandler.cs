
namespace KafkaSignalR.Domain.Model.Kafka
{
    public interface IPubSubHandler<TEventStream>
        where TEventStream : EventStream
    {
        void Handle(TEventStream stream);
    }
}
