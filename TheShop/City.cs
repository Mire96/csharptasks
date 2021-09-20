using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Dapper;

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

        public void InsertShops()
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                //string selectProductByName = $"SELECT * FROM public.ItemValueRecord where name = '{itemValueRecord.Item.Name()}'";
                //connection.Query<ItemValueRecord>(selectProductByName).Single();

                //itemValueRecord.Quantity = item.Quantity;
                //itemValueRecord.Date = DateTime.Today.ToString("yyyy-MM-dd");
                //connection.BeginTransaction();
                if (StoreList.Any())
                {
                    foreach (Store store in StoreList)
                    {
                        string insertrecordInInventory = $"INSERT INTO shopdb.store(cityname, name, address)VALUES ('{store.CityName}', '{store.Name}', '{store.Address}');";
                        connection.Execute(insertrecordInInventory);
                    }
                }

            }
        }

        public List<Store> GetStoreList()
        {
            List<Store> stores = new List<Store>();

            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string selectStoreByCityName = $"SELECT * FROM shopdb.store where cityname = '{Name}'";
                stores = (List<Store>)connection.Query<Store>(selectStoreByCityName);
            }


            return stores;
        }
    }
}
