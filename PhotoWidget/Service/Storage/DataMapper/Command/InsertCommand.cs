using System.Linq;
using Npgsql;
using PhotoWidget.Service.Helper.Attributes;
using PhotoWidget.Service.Storage.DataMapper.Query;

namespace PhotoWidget.Service.Storage.DataMapper.Command
{
    public class InsertCommand<T>
    {
        private readonly SqlQuery<T> _query;

        public InsertCommand(SqlQuery<T> query)
        {
            _query = query;
        }

        public NpgsqlCommand Single(T entity)
        {
            var sql = _query.Insert.Single(x => !x.column.IsSetByStorage);

            var command = new NpgsqlCommand(sql);

            foreach (var extProperty in ColumnHelper.GetColumnProperties<T>().Where(x => !x.column.IsSetByStorage)) {
                command.Parameters.AddWithValue(extProperty.property.Name, extProperty.column.Type, extProperty.property.GetValue(entity));
            }

            return command;
        }
    }
}