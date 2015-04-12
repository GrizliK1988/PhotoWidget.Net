using Npgsql;

namespace PhotoWidget.Service.Storage
{
    public class ConnectionManager : IConnectionManager
    {
        private NpgsqlConnection _connection;

        public NpgsqlConnection GetConnection()
        {
            if (_connection == null) {
                _connection = ConnectionFactory.Create();
                _connection.Open();
            }

            return _connection;
        }
    }
}