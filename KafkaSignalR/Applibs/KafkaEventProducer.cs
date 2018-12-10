
namespace KafkaSignalR.Applibs
{
    using System;
    using System.Collections.Generic;
    using Confluent.Kafka;
    using Confluent.Kafka.Serialization;
    using KafkaSignalR.Domain.Model.Kafka;
    using Newtonsoft.Json;

    public class KafkaEventProducer
    {
        // 0: The message was received, but not committed
        // 1: The leader has written the message to disk
        //-1: The leader has received confirmation from all replicas that the data has been written to disk
        private static Dictionary<string, object> constructConfig(string brokerList, int requiredAcks) =>
            new Dictionary<string, object>
            {
                 { "bootstrap.servers", brokerList },
                // extra
                { "message.send.max.retries", 3 },
                { "default.topic.config", new Dictionary<string, object>()
                    {
                       { "request.required.acks", requiredAcks },
                    }
                }
            };

        private Producer<string, string> publisher;

        private static Lazy<KafkaEventProducer> lazyEventProducer;

        public static KafkaEventProducer Producer
        {
            get
            {
                return lazyEventProducer.Value;
            }
        }

        private KafkaEventProducer(Producer<string, string> publisher)
        {
            this.publisher = publisher;
        }

        public static void Start(string brokerList, int requiredAcks)
        {
            lazyEventProducer = new Lazy<KafkaEventProducer>(() =>
            {
                return new KafkaEventProducer(new Producer<string, string>(constructConfig(brokerList, requiredAcks), new StringSerializer(System.Text.Encoding.UTF8), new StringSerializer(System.Text.Encoding.UTF8)));
            });
        }

        public void Publish<T>(string topicName, int suffix, T data)
        {
            var es = new KafkaEventStream(
                typeof(T).Name, 
                JsonConvert.SerializeObject(data), 
                TimeStampHelper.ToUtcTimeStamp(DateTime.Now));

            publisher?.ProduceAsync(topicName, $"{es.Type}.{suffix}", JsonConvert.SerializeObject(es))
                .ConfigureAwait(false);
            /*deliveryReport?.ContinueWith(task =>
            {
                //communicator.getLogger().trace("", $"Partition: {task.Result.Partition}, Offset: {task.Result.Offset}");
            });*/
        }

        public static void Stop()
        {
            if (lazyEventProducer.IsValueCreated)
            {
                lazyEventProducer.Value.Flush();
                lazyEventProducer = null;
            }
        }

        public void Flush()
        {
            publisher?.Flush(1000);
            publisher?.Dispose();
            publisher = null;
        }
    }
}