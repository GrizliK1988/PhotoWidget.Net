using System;
using System.Linq;
using System.Reflection;
using PhotoWidget.Attributes;

namespace PhotoWidget.Service.Helper.Attributes
{
    public class ColumnHelper
    {
        public static PropertyInfoWithAttributes[] GetColumnProperties<T>()
        {
            var type = typeof (T);
            return type.GetProperties().Where(HasColumnAttribute)
                .Select(x => new PropertyInfoWithAttributes() { property = x, column = GetColumnAttribute(x) })
                .ToArray();
        }

        public static Column[] GetColumnAttributes(Type type)
        {
            return type.GetProperties().Where(HasColumnAttribute).Select(GetColumnAttribute).ToArray();
        }

        public static Column GetColumnAttribute(MemberInfo property)
        {
            return (Column)property.GetCustomAttribute(typeof(Column));
        }

        public static bool IsSetByStorage(MemberInfo property)
        {
            var column = (Column)property.GetCustomAttribute(typeof(Column));
            return column.IsSetByStorage;
        }

        public static bool IsPrimaryKey(MemberInfo property)
        {
            var column = (Column)property.GetCustomAttribute(typeof(Column));
            return column.IsPrimaryKey;
        }

        public static bool HasColumnAttribute(MemberInfo property)
        {
            return property.GetCustomAttributes(typeof(Column)).Count() == 1;
        }
    }

    public struct PropertyInfoWithAttributes
    {
        public PropertyInfo property;

        public Column column;
    }
}