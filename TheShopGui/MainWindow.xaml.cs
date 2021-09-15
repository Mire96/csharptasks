using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary;
using Npgsql;
using Dapper;

namespace TheShopGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Store> StoresInCity;
        Inventory inventory;
        public MainWindow()
        {
            InitializeComponent();
            StoresInCity = new List<Store>();
            inventory = new Inventory();
            dgridStoresInCity.ItemsSource = StoresInCity;
        }

        private void btnAddNewStore_Click(object sender, RoutedEventArgs e)
        {
            //Store store = new Store(tfCityName.Text, tfStoreName.Text, tfStoreAddress.Text);
            //StoresInCity.Add(store);
            Store shop1 = new Store("Beograd", "Aman", "Tadeusa Koscuska");
            Store shop2 = new Store("Beograd", "Maxi", "Maksima Gorkog");
            Store shop3 = new Store("Beograd", "Idea", "Kraljice Marije");

            StoresInCity.Add(shop1);
            StoresInCity.Add(shop2);
            StoresInCity.Add(shop3);
        }

        private void btnFinishStoreAdding_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                //string selectProductByName = $"SELECT * FROM public.ItemValueRecord where name = '{itemValueRecord.Item.Name()}'";
                //connection.Query<ItemValueRecord>(selectProductByName).Single();

                //itemValueRecord.Quantity = item.Quantity;
                //itemValueRecord.Date = DateTime.Today.ToString("yyyy-MM-dd");
                //connection.BeginTransaction();
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

        private void btnAddNewInventoryItem_Click(object sender, RoutedEventArgs e)
        {
            Item i = new Item("Eggs", UnitMeasure.PIECES);
            Item i2 = new Item("Milk", UnitMeasure.LITER);
            Item i3 = new Item("Flour", UnitMeasure.KG);
            Item i4 = new Item("Orange juice", UnitMeasure.LITER);
            ItemValueRecord eggs = new ItemValueRecord(i, 12, 12);
            ItemValueRecord milk = new ItemValueRecord(i2, 20, 100);
            ItemValueRecord flour = new ItemValueRecord(i3, 6, 50);
            ItemValueRecord orangeJuice = new ItemValueRecord(i4, 6, 150);

            inventory.AddRecord(eggs);
            inventory.AddRecord(milk);
            inventory.AddRecord(flour);
            inventory.AddRecord(orangeJuice);
        }

        private void btnFinishInventoring_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                //string selectProductByName = $"SELECT * FROM public.ItemValueRecord where name = '{itemValueRecord.Item.Name()}'";
                //connection.Query<ItemValueRecord>(selectProductByName).Single();

                //itemValueRecord.Quantity = item.Quantity;
                //itemValueRecord.Date = DateTime.Today.ToString("yyyy-MM-dd");
                //connection.BeginTransaction();
                int productId = 1;
                if (inventory.ItemList.Any())
                {
                    foreach (ItemValueRecord itemValueRecord in inventory.ItemList)
                    {
                        itemValueRecord.Date = DateTime.Today.ToString("yyyy-MM-dd");

                        string insertRecord = $"INSERT INTO shopdb.product(name, unit, unitprice)VALUES('{itemValueRecord.Item.Name}', '{itemValueRecord.Item.Unit.ToString()}',  {itemValueRecord.UnitPrice});";
                        connection.Execute(insertRecord);


                        string insertInventory = $"INSERT INTO shopdb.inventory(id, productid, quantity, date)VALUES ({inventory.Id}, {productId}, {itemValueRecord.Quantity}, '{itemValueRecord.Date}');";
                        connection.Execute(insertInventory);
                        productId++;
                    }
                }
            }
        }
    }
}
