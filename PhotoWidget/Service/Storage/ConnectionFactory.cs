using Npgsql;

namespace PhotoWidget.Service.Storage
{
    public class ConnectionFactory
    {
        public static NpgsqlConnection Create()
        {
            var connectionBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Port = 5432,
                UserName = "postgres",
                Password = "260288",
                Database = "photo_widget"
            };
            return new NpgsqlConnection(connectionBuilder);
        }
    }
}