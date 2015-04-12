using System;
using NpgsqlTypes;

namespace PhotoWidget.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Column : Attribute
    {
        public string Name { get; private set; }

        public NpgsqlDbType Type { get; private set; }

        public bool IsSetByStorage { get; set; }

        public bool IsPrimaryKey { get; set; }

        public Column(string name, NpgsqlDbType type)
        {
            Name = name;
            Type = type;
        }
    }
}