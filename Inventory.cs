using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Dapper;

namespace TheShop
{
    public class Inventory
    {

        

        public List<ItemValueRecord> ItemList { get; set; }

        public Inventory()
        {
            ItemList = new List<ItemValueRecord>();
        }

        public void AddRecord(ItemValueRecord itemValueRecord)
        {
            ItemList.Add(itemValueRecord);
        }

        public ItemValueRecord RetrieveRecord (string name)
        {
            foreach(ItemValueRecord i in ItemList)
            {
                if (i.Item.Name.Equals(name))
                {
                    return i;
                }
            }

            return null;
        }

        public void GroupInsertIntoTable()
        {
            foreach (ItemValueRecord itemValueRecord in ItemList)
            {
                string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    //string selectProductByName = $"SELECT * FROM public.ItemValueRecord where name = '{itemValueRecord.Item.Name()}'";
                    //connection.Query<ItemValueRecord>(selectProductByName).Single();

                    //itemValueRecord.Quantity = item.Quantity;
                    //itemValueRecord.Date = DateTime.Today.ToString("yyyy-MM-dd");
                    //connection.BeginTransaction();


                    string insertrecordInInventory = $"INSERT INTO shopdb.product(name, unit, quantity, unitprice)VALUES('{itemValueRecord.Item.Name}', '{itemValueRecord.Item.Unit.ToString()}', {itemValueRecord.Quantity}, {itemValueRecord.UnitPrice});";
                    connection.Execute(insertrecordInInventory);

                }
            }
            
        }

        public override string ToString()
        {
            string returnString = $"Total inventory value: {TotalValueInStorage()}\n";
            
            foreach(ItemValueRecord i in ItemList)
            {
                returnString += i + "\n";
            }
            return returnString;
        }

        public double TotalValueInStorage()
        {
            double result = 0;

            foreach(ItemValueRecord i in ItemList)
            {
                result += i.CalculateValue();
            }
            return result;
        }
    }
}
