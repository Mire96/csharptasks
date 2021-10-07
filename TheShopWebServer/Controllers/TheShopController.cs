using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using ClassLibrary;

namespace TheShopWebServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheShopController : Controller
    {

        [HttpGet]
        [Route("hello")]
        public string GetHelloWorld()
        {
            return "Hello World";
        }


        [HttpGet]
        [Route("{cityname}/Stores")]
        public List<Store> GetStoreList(string cityname)
        {
            List<Store> stores = new List<Store>();

            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string selectStoreByCityName = $"SELECT * FROM shopdb.store where cityname = '{cityname}'";
                stores = (List<Store>)connection.Query<Store>(selectStoreByCityName);
            }
            GetInventories(stores);


            return stores;
        }

        private void GetInventories(List<Store> storesInCity)
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

        private void GetItemValueRecords(Inventory inventory)
        {

            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string selectStoreByCityName = $"SELECT * FROM shopdb.product where inventoryid = {inventory.Id}";
                inventory.ItemList = (List<ItemValueRecord>)connection.Query<ItemValueRecord>(selectStoreByCityName);
            }

        }

        [HttpPost]
        [Route("insert/stores")]
        public void InsertStores([FromBody] List<Store> StoresInCity)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                if (StoresInCity.Any())
                {
                    foreach (Store store in StoresInCity)
                    {
                        string insertrecordInInventory = $"INSERT INTO shopdb.store(cityname, name, address)VALUES ('{store.CityName}', '{store.Name}', '{store.Address}');";
                        connection.Execute(insertrecordInInventory);
                    }
                }
            }
        }

        [HttpPost]
        [Route("insert/inventory/{storename}")]
        public void InsertInventory (string storename, Inventory inventory)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                
                
                if (inventory.ItemList.Any() && storename != null)
                {

                    //store.Inventory = inventory;
                    string insertInventory = $"INSERT INTO shopdb.inventory(storename)VALUES ('{storename}');";
                    connection.Execute(insertInventory);
                    foreach (ItemValueRecord itemValueRecord in inventory.ItemList)
                    {
                        itemValueRecord.Date = DateTime.Today.ToString("yyyy-MM-dd");

                        string insertRecord = $"INSERT INTO shopdb.product(id, name, unit, unitprice, quantity, inventoryid, date)VALUES({itemValueRecord.Id}, '{itemValueRecord.Item.Name}', '{itemValueRecord.Item.Unit.ToString()}',  {itemValueRecord.UnitPrice}, {itemValueRecord.Quantity}, {inventory.Id}, '{itemValueRecord.Date}');";
                        connection.Execute(insertRecord);

                        
                    }

                }
                
            }
        }

        [HttpPut]
        [Route("update/stores/{id}")]
        public void UpdateStoreAddress(int id, string address)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string updateStore = $"UPDATE shopdb.store SET address = '{address}' WHERE id = {id};";
                connection.Execute(updateStore);
            }
        }

        [HttpPut]
        [Route("update/inventory/itemrecordquantity/{id}")]
        public void UpdateItemRecordQuantity(int id, double quantity)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string updateProduct = $"UPDATE shopdb.product SET quantity = '{quantity}' WHERE id = {id};";
                connection.Execute(updateProduct);
            }
        }

        [HttpDelete]
        [Route("delete/inventory/itemrecord/{itemname}")]
        public void DeleteItemFromStore(string itemname, string storename)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string deleteItemFromStore = $"DELETE FROM shopdb.product prod USING shopdb.inventory inv WHERE prod.inventoryid = inv.id AND inv.storename = '{storename}' and prod.name = '{itemname}';";
                connection.Execute(deleteItemFromStore);
            }
        }

        [HttpDelete]
        [Route("delete/store/{id}")]
        public void DeleteStore(int id, string storename)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string deleteItemFromStore = $"DELETE FROM shopdb.product prod USING shopdb.inventory inv WHERE prod.inventoryid = inv.id AND inv.storename = '{storename}';";
                connection.Execute(deleteItemFromStore);
                string deleteInventoryFromStore = $"DELETE FROM shopdb.inventory WHERE storename = '{storename}';";
                connection.Execute(deleteInventoryFromStore);
                string deleteStore = $"DELETE FROM shopdb.store WHERE id = '{id}';";
                connection.Execute(deleteStore);
            }
        }
    }
}
