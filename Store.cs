using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheShop
{
    public class Store
    {
        public string CityName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Inventory Inventory { get; set; }

        public Store(string name, string address)
        {
            Name = name;
            Address = address;
            Inventory = new Inventory();
        }

        public void AddRecord(ItemValueRecord item)
        {
            Inventory.AddRecord(item);
        }

        public void GroupInsertIntoTable()
        {
            Inventory.GroupInsertIntoTable();
        }

        public override string ToString()
        {
            return $"Store name: {Name}, Address: {Address} \n{Inventory}";
        }
    }
}
