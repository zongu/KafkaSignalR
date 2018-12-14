
namespace KafkaSignalR.Event
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Autofac;
    using Common.Logging;
    using KafkaSignalR.Domain.Model;
    using KafkaSignalR.Domain.Model.Event;
    using KafkaSignalR.Domain.Model.Kafka;
    using KafkaSignalR.Domain.Persistent;
    using Newtonsoft.Json;

    public class PubMessageEventHandler : IkafkaPubSubHandler
    {
        private ILog log = LogManager.GetLogger<PubMessageEventHandler>();

        public void Handle(KafkaEventStream stream)
        {
            try
            {
                this.log.Info($"{this.GetType().Name} Data:{stream.Data}");
                using (var scope = Applibs.AutoFacConfig.Container.BeginLifetimeScope())
                {
                    var @event = JsonConvert.DeserializeObject<PubMessageEvent>(stream.Data);
                    var repo = scope.Resolve<IPubMessageRepository>();

                    var result = repo.Add(new PubMessage()
                    {
                        Content = @event.Content
                    });

                    if(result.Item1 != null)
                    {
                        throw result.Item1;
                    }
                }
            }
            catch (Exception ex)
            {
                this.log.Error($"PubMessageEventHandler Exception: {ex.Message}");
            }
        }
    }
}