using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheShop
{
    public class City
    {
        public string Name { get; set; }
        public List<Store> StoreList { get; set; }

        public City(string name)
        {
            this.Name = name;
            this.StoreList = new List<Store>();
        }

        public void AddStore(Store shop)
        {
            StoreList.Add(shop);
            shop.CityName = this.Name;
        }
    }
}
