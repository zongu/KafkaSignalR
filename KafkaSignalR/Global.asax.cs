
namespace KafkaSignalR
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using KafkaSignalR.Applibs;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            KafkaEventProducer.Start(ConfigHelper.KafkaBorkerList, ConfigHelper.KafkaProducerRequiredAcks);
        }

        protected void Application_End()
        {
            KafkaEventProducer.Stop();
        }
    }
}
