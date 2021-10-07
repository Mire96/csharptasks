using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Item : IItem
    {
        public string Name { get; set; }
        public UnitMeasure Unit { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   Name == item.Name;
        }

        public Item(string name, UnitMeasure unit)
        {
            Name = name;
            Unit = unit;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
