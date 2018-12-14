
namespace KafkaSignalR
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using KafkaSignalR.Applibs;
    using KafkaSignalR.Domain.Model.Kafka;

    public class WebApiApplication : System.Web.HttpApplication
    {
        private ConsumerGroup consumerGroup;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoFacConfig.ContainerGenerate();
            KafkaEventProducer.Start(
                ConfigHelper.KafkaBorkerList, 
                ConfigHelper.KafkaProducerRequiredAcks);

            consumerGroup = new ConsumerGroup(
                ConfigHelper.KafkaConsumerNumbers,
                ConfigHelper.KafkaConsumerGroupId,
                ConfigHelper.KafkaConsumerTopics,
                ConfigHelper.KafkaBorkerList,
                new PubSubDispatcher<KafkaEventStream>(AutoFacConfig.Container));
            consumerGroup.Start();
        }

        protected void Application_End()
        {
            KafkaEventProducer.Stop();
            consumerGroup.Stop();
        }
    }
}
