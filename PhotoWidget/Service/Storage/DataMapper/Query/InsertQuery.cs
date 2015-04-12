using System;
using System.Linq;
using System.Reflection;
using PhotoWidget.Attributes;
using PhotoWidget.Service.Helper.Attributes;

namespace PhotoWidget.Service.Storage.DataMapper.Query
{
    public class InsertQuery<T>
    {
        public string Single(Func<PropertyInfoWithAttributes, bool> insertFieldFilter)
        {
            var type = typeof (T);
            var table = (Table) type.GetCustomAttribute(typeof (Table));

            var insertExtProperties = ColumnHelper.GetColumnProperties<T>().Where(insertFieldFilter).ToArray();
            var columns = string.Join(", ", insertExtProperties.Select(x => x.column.Name));
            var parameters = string.Join(", ", insertExtProperties.Select(x => ":" + x.property.Name));
            var setByStorageProperties = ColumnHelper.GetColumnProperties<T>().Where(x => x.column.IsSetByStorage);
            var returningFields = string.Join(", ", setByStorageProperties.Select(x => x.column.Name));

            var sql = string.Format("INSERT INTO {0}({1}) VALUES({2}) RETURNING {3}", table.Name, columns, parameters, returningFields);
            return sql;
        }
    }
}