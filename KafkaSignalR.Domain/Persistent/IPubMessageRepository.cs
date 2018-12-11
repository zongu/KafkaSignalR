
namespace KafkaSignalR.Domain.Persistent
{
    using System;
    using KafkaSignalR.Domain.Model;

    public interface IPubMessageRepository
    {
        Tuple<Exception, PubMessage> Add(PubMessage message);
    }
}
