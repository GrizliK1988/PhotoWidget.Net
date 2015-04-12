using Npgsql;

namespace PhotoWidget.Service.Storage
{
    public interface IConnectionManager
    {
        NpgsqlConnection GetConnection();
    }
}
