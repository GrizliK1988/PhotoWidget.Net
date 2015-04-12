using System;
using System.Linq;
using System.Reflection;
using PhotoWidget.Attributes;
using PhotoWidget.Service.Helper.Attributes;

namespace PhotoWidget.Service.Storage.DataMapper.Query
{
    public class UpdateQuery<T>
    {
        public string ById(Func<PropertyInfoWithAttributes, bool> updateFieldFilter)
        {
            var type = typeof (T);
            var table = (Table) type.GetCustomAttribute(typeof (Table));

            var updateExtProperties = ColumnHelper.GetColumnProperties<T>().Where(updateFieldFilter).ToArray();
            var updateParts = string.Join(", ", updateExtProperties.Select(x => x.column.Name + " = :" + x.property.Name));

            var pkProperty = ColumnHelper.GetColumnProperties<T>().First(x => x.column.IsPrimaryKey);
            var wherePart = string.Format("{0} = :{1}", pkProperty.column.Name, pkProperty.property.Name);

            var setByStorageProperties = ColumnHelper.GetColumnProperties<T>().Where(x => x.column.IsSetByStorage);
            var returningFields = string.Join(", ", setByStorageProperties.Select(x => x.column.Name));

            var sql = string.Format("UPDATE {0} SET {1} WHERE {2} RETURNING {3}", table.Name, updateParts, wherePart, returningFields);
            return sql;
        }
    }
}