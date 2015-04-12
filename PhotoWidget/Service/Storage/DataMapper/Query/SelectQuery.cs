using System.Reflection;
using PhotoWidget.Attributes;
using PhotoWidget.Service.Helper.Attributes;

namespace PhotoWidget.Service.Storage.DataMapper.Query
{
    public class SelectQuery<T>
    {
        public string All()
        {
            var type = typeof (T);
            var table = (Table) type.GetCustomAttribute(typeof (Table));

            var sql = string.Format("SELECT * FROM {0}", table.Name);
            return sql;
        }

        public string ByProperty(MemberInfo property)
        {
            var type = typeof (T);
            var table = (Table) type.GetCustomAttribute(typeof (Table));

            var column = ColumnHelper.GetColumnAttribute(property);
            var whereSql = string.Format("{0} = :{1}", column.Name, property.Name);

            var sql = string.Format("SELECT * FROM {0} WHERE {1}", table.Name, whereSql);
            return sql;
        }
    }
}