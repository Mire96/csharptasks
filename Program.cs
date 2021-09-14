using System;

namespace TheShop
{
    class Program
    {
        static void Main(string[] args)
        {
            Item i = new Item("Eggs", UnitMeasure.PIECES);
            Item i2 = new Item("Milk", UnitMeasure.LITER);
            Item i3 = new Item("Flour", UnitMeasure.KG);
            Item i4 = new Item("Orange juice", UnitMeasure.LITER);
            ItemValueRecord eggs = new ItemValueRecord(i, 12, 12);
            ItemValueRecord milk = new ItemValueRecord(i2, 20, 100);
            ItemValueRecord flour = new ItemValueRecord(i3, 6, 50);
            ItemValueRecord orangeJuice = new ItemValueRecord(i4, 6, 150);

            City city = new City("Beograd");
            Store shop = new Store("Aman", "Tadeusa Koscuska");
            city.AddStore(shop);

            shop.AddRecord(eggs);
            shop.AddRecord(milk);
            shop.AddRecord(flour);
            shop.AddRecord(orangeJuice);

            Console.WriteLine(shop);

            shop.GroupInsertIntoTable();
            city.InsertShops();
            

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
    }
}
