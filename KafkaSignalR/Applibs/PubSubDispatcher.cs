
namespace KafkaSignalR.Applibs
{
    using Autofac;
    using Common.Logging;
    using KafkaSignalR.Domain.Model.Kafka;

    public class PubSubDispatcher<TEventStream> : IPubSubDispatcher<TEventStream>
            where TEventStream : EventStream
    {
        private IContainer container;
        private ILog logger = LogManager.GetLogger<PubSubDispatcher<TEventStream>>();

        public PubSubDispatcher(IContainer container)
        {
            this.container = container;
        }

        public void DispatchMessage(TEventStream stream)
        {
            try
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    var handler = scope.ResolveNamed<IPubSubHandler<TEventStream>>(stream.Type);
                    handler?.Handle(stream);
                }
            }
            catch (Autofac.Core.Registration.ComponentNotRegisteredException)
            {
                // ignore
            }
            catch (System.Exception ex)
            {
                logger.Warn($"DispatchMessage {stream.Type} is exception, Result:{ex.Message}");
            }
        }

        public void DispatchError(string error)
            => logger.Warn($"DispatchMessage is error, Result:{error}");

        public void WriteInfoLog(string info)
            => logger.Info($"PubSubDispatcher WriteInfoLog {info}");

        public void WriteWarnLog(string warn)
            => logger.Warn($"PubSubDispatcher WriteInfoLog {warn}");

        public void WriteErrorLog(string error)
            => logger.Error($"PubSubDispatcher WriteErrorLog {error}");
    }
}