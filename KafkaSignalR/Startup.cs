
[assembly: Microsoft.Owin.OwinStartupAttribute(typeof(KafkaSignalR.Startup))]
namespace KafkaSignalR
{
    using Microsoft.Owin.Cors;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}