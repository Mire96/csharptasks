using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheShop
{
    interface IItem
    {
        public string Name { get; set; }

        public UnitMeasure Unit { get; set; }
    }
}
