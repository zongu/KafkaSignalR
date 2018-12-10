
namespace KafkaSignalR.Domain.Model.Kafka
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
