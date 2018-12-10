
namespace KafkaSignalR.Applibs
{
    using System.Configuration;

    public static class ConfigHelper
    {
        public static readonly string KafkaBorkerList = ConfigurationManager.AppSettings["Kafka.BorkerList"];

        public static readonly int KafkaProducerRequiredAcks = int.Parse(ConfigurationManager.AppSettings["Kafka.KafkaProducerRequiredAcks"]);
    }
}