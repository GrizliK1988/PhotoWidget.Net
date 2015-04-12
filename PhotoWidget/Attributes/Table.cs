using System;

namespace PhotoWidget.Attributes
{
    public class Table : Attribute
    {
        public string Name { get; private set; }

        public Table(string name)
        {
            Name = name;
        }
    }
}