
namespace KafkaSignalR.Applibs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Confluent.Kafka.Serialization;
    using KafkaSignalR.Domain.Model.Kafka;
    using Newtonsoft.Json;

    public class ConsumerGroup
    {
        private List<KafkaEventConsumer> consumers = new List<KafkaEventConsumer>();

        public ConsumerGroup(int consumerNum, string groupId, IEnumerable<string> topics, string brokerList, IPubSubDispatcher<KafkaEventStream> dispatcher)
        {
            for (int i = 0; i < consumerNum; i++)
            {
                var c = new KafkaEventConsumer(groupId, topics, brokerList, dispatcher);
                consumers.Add(c);
            }
        }

        public void Start()
        {
            foreach (var c in consumers)
            {
                c.Start();
            }
        }

        public void Stop()
        {
            Parallel.ForEach(consumers, c =>
            {
                c.Stop();
            });
        }
    }

    public class KafkaEventConsumer
    {
        private Dictionary<string, object> constructConfig(string groupId, string brokerList, bool enableAutoCommit) =>
            new Dictionary<string, object>
            {
                { "group.id", groupId },
                { "enable.auto.commit", enableAutoCommit },
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
                { "bootstrap.servers", brokerList },
                { "default.topic.config", new Dictionary<string, object>()
                    {
                        { "auto.offset.reset", "latest" }
                    }
                }
            };

        private IPubSubDispatcher<KafkaEventStream> dispatcher;

        private Consumer<string, string> consumer;

        private IEnumerable<string> topics;

        private Thread thread;

        private bool running;

        public KafkaEventConsumer(string groupId, IEnumerable<string> topics, string brokerList, IPubSubDispatcher<KafkaEventStream> dispatcher)
        {
            consumer = new Consumer<string, string>(constructConfig(groupId, brokerList, true), new StringDeserializer(System.Text.Encoding.UTF8), new StringDeserializer(System.Text.Encoding.UTF8));
            this.topics = topics;
            running = false;
            this.dispatcher = dispatcher;
        }

        public void Start()
        {
            if (thread == null)
            {
                thread = new Thread(new ThreadStart(Run));
                thread.IsBackground = true;
                thread.Start();

                running = true;
            }
        }

        public void Stop()
        {
            running = false;
            thread?.Join();
            thread = null;
        }

        private void Run()
        {
            {
                // Note: All event handlers are called on the main thread.
                consumer.OnMessage += (_, msg) =>
                {
                    var @event = JsonConvert.DeserializeObject<KafkaEventStream>(msg.Value);
                    dispatcher?.DispatchMessage(@event);
                };

                consumer.OnError += (_, error)
                    => dispatcher?.WriteErrorLog(error.Reason);

                consumer.OnPartitionsAssigned += (_, partitions) =>
                {
                    consumer.Assign(partitions);
                };

                consumer.OnPartitionsRevoked += (_, partitions) =>
                {
                    consumer.Unassign();
                };

                /*consumer.OnStatistics += (_, json)
                    => Console.WriteLine($"Statistics: {json}");*/

                consumer.Subscribe(topics);

                //Console.WriteLine($"Subscribed to: [{string.Join(", ", consumer.Subscription)}]");

                /*var cancelled = false;
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };*/

                try
                {
                    while (running)
                    {
                        consumer?.Poll(TimeSpan.FromMilliseconds(1000));
                    }

                    consumer?.Dispose();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}