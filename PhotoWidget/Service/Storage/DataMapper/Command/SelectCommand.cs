using System.Reflection;
using Npgsql;
using PhotoWidget.Service.Helper.Attributes;
using PhotoWidget.Service.Storage.DataMapper.Query;

namespace PhotoWidget.Service.Storage.DataMapper.Command
{
    public class SelectCommand<T>
    {
        private readonly SqlQuery<T> _query;

        public SelectCommand(SqlQuery<T> query)
        {
            _query = query;
        }

        public NpgsqlCommand All()
        {
            var sql = _query.Select.All();

            var command = new NpgsqlCommand(sql);
            return command;
        }

        public NpgsqlCommand ByProperty(MemberInfo property, object value)
        {
            var sql = _query.Select.ByProperty(property);
            var column = ColumnHelper.GetColumnAttribute(property);

            var command = new NpgsqlCommand(sql);
            command.Parameters.AddWithValue(property.Name, column.Type, value);

            return command;
        }
    }
}