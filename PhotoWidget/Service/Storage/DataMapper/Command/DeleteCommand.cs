using System.Linq;
using Npgsql;
using PhotoWidget.Service.Helper.Attributes;
using PhotoWidget.Service.Storage.DataMapper.Query;

namespace PhotoWidget.Service.Storage.DataMapper.Command
{
    public class DeleteCommand<T>
    {
        private readonly SqlQuery<T> _query;

        public DeleteCommand(SqlQuery<T> query)
        {
            _query = query;
        }

        public NpgsqlCommand ById(object id)
        {
            var sql = _query.Delete.ById();

            var command = new NpgsqlCommand(sql);

            var pkProperty = ColumnHelper.GetColumnProperties<T>().First(x => x.column.IsPrimaryKey);
            command.Parameters.AddWithValue(pkProperty.property.Name, pkProperty.column.Type, id);

            return command;
        }
    }
}