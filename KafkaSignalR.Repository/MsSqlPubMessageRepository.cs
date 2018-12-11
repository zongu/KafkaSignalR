
namespace KafkaSignalR.Repository
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
    using KafkaSignalR.Domain.Model;
    using KafkaSignalR.Domain.Persistent;

    public class MsSqlPubMessageRepository : IPubMessageRepository
    {
        private string _connectionString;

        public MsSqlPubMessageRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public Tuple<Exception, PubMessage> Add(PubMessage message)
        {
            try
            {
                using (var cn = new SqlConnection(this._connectionString))
                {
                    var result = cn.QueryFirstOrDefault<PubMessage>(
                        "NSP_PubMessage_Add",
                        new
                        {
                            message.Content
                        },
                        commandType: CommandType.StoredProcedure);

                    return Tuple.Create<Exception, PubMessage>(null, result);
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create<Exception, PubMessage>(ex, null);
            }
        }
    }
}
