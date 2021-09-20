using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using Dapper;

namespace TheShop
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Store> StoresInCity = new List<Store>();
            Inventory inventory;
            StoresInCity = GetStoreList("Beograd");
            inventory = new Inventory();
            

            GetInventories(StoresInCity);

            PrintDatabase(StoresInCity);


            //Item i = new Item("Eggs", UnitMeasure.PIECES);
            //Item i2 = new Item("Milk", UnitMeasure.LITER);
            //Item i3 = new Item("Flour", UnitMeasure.KG);
            //Item i4 = new Item("Orange juice", UnitMeasure.LITER);
            //ItemValueRecord eggs = new ItemValueRecord(i, 12, 12);
            //ItemValueRecord milk = new ItemValueRecord(i2, 20, 100);
            //ItemValueRecord flour = new ItemValueRecord(i3, 6, 50);
            //ItemValueRecord orangeJuice = new ItemValueRecord(i4, 6, 150);

            //City city = new City("Beograd");
            //Store shop = new Store("Aman", "Tadeusa Koscuska");
            //city.AddStore(shop);

            //shop.AddRecord(eggs);
            //shop.AddRecord(milk);
            //shop.AddRecord(flour);
            //shop.AddRecord(orangeJuice);

            //Console.WriteLine(shop);

            //shop.GroupInsertIntoTable();
            //city.InsertShops();
            

            while (true)
            {
                Console.WriteLine("Input next item in this format: Item name,Unit measure(kg,liters,pieces),Quantity,Price ");
                string itemValueRecordString = Console.ReadLine();
                try
                {   string name = itemValueRecordString.Split(" ")[0];
                    string unitMeasure = itemValueRecordString.Split(" ")[1];
                    string quantity = itemValueRecordString.Split(" ")[2];
                    string price = itemValueRecordString.Split(" ")[3];
                    new ItemValueRecord(name, unitMeasure, quantity, price);
                }
                catch (Exception)
                {
                    Console.WriteLine("Format input is wrong");
                    
                }
            }
        }

        private static void PrintDatabase(List<Store> StoresInCity)
        {
            foreach (Store store in StoresInCity)
            {
                Console.WriteLine(store);
            }
        }

        private static void GetInventories(List<Store> storesInCity)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                foreach (Store store in storesInCity)
                {
                    string selectInventoryByStoreName = $"SELECT * FROM shopdb.inventory where storename = '{store.Name}'";
                    Inventory inventory = connection.Query<Inventory>(selectInventoryByStoreName).SingleOrDefault();
                    if (inventory == null)
                    {
                        inventory = new Inventory(0, store.Name);
                    }
                    store.Inventory = inventory;

                    GetItemValueRecords(store.Inventory);
                }
            }
        }

        private static void GetItemValueRecords(Inventory inventory)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string selectStoreByCityName = $"SELECT * FROM shopdb.product where inventoryid = {inventory.Id}";
                inventory.ItemList = (List<ItemValueRecord>)connection.Query<ItemValueRecord>(selectStoreByCityName);
            }

            Console.WriteLine("ItemValueRecords successfuly retrieved from database!");
        }

        private static List<Store> GetStoreList(string cityname)
        {
            List<Store> stores = new List<Store>();

            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string selectStoreByCityName = $"SELECT * FROM shopdb.store where cityname = '{cityname}'";
                stores = (List<Store>)connection.Query<Store>(selectStoreByCityName);
            }

            Console.WriteLine("Stores successfuly retrieved from database!");


            return stores;
        }
    }
}
