using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Store> StoresInCity = new ObservableCollection<Store>();
        Inventory inventory;
        ObservableCollection<ItemValueRecord> InventoryItems = new ObservableCollection<ItemValueRecord>();
        public MainWindow()
        {
            InitializeComponent();
            StoresInCity = new ObservableCollection<Store>(GetStoreList("Beograd"));
            //inventory = new Inventory();
            AssignInventory(InventoryItems);
            dgridStoresInCity.ItemsSource = StoresInCity;


            
            GetInventories(StoresInCity.ToList());
        }

        private void AssignInventory(ObservableCollection<ItemValueRecord> InventoryItems)
        {
            InventoryItems = new ObservableCollection<ItemValueRecord>(new List<ItemValueRecord>());
            if(dgridStoresInCity.SelectedItem != null)
            {
                Store store = (Store)dgridStoresInCity.SelectedItem;
                InventoryItems = new ObservableCollection<ItemValueRecord>(store.Inventory.ItemList);
            }

            dgridInventory.ItemsSource = InventoryItems;

            
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
                    if(inventory == null)
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

            MessageBox.Show("ItemValueRecords successfuly retrieved from database!");
        }

        private void btnAddNewStore_Click(object sender, RoutedEventArgs e)
        {
            Store store = new Store(tfCityName.Text, tfStoreName.Text, tfStoreAddress.Text);
            StoresInCity.Add(store);
            //Store shop1 = new Store("Beograd", "Aman", "Tadeusa Koscuska");
            //Store shop2 = new Store("Beograd", "Maxi", "Maksima Gorkog");
            //Store shop3 = new Store("Beograd", "Idea", "Kraljice Marije");

            //StoresInCity.Add(shop1);
            //StoresInCity.Add(shop2);
            //StoresInCity.Add(shop3);

            
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

                    MessageBox.Show("Stores successfuly added to database!");
                }

            }

            

        }

        private List<Store> GetStoreList(string cityname) 
        {
            List<Store> stores = new List<Store>();

            string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=shopDb;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string selectStoreByCityName = $"SELECT * FROM shopdb.store where cityname = '{cityname}'";
                stores = (List<Store>) connection.Query<Store>(selectStoreByCityName);
            }

            MessageBox.Show("Stores successfuly retrieved from database!");


            return stores;
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

            MessageBox.Show("Items successfuly added to inventory!");
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
                Store store = (Store)dgridStoresInCity.SelectedItem;
                if (inventory.ItemList.Any() && store != null)
                {

                    store.Inventory = inventory;
                    string insertInventory = $"INSERT INTO shopdb.inventory(storename)VALUES ('{store.Name}');";
                    connection.Execute(insertInventory);
                    foreach (ItemValueRecord itemValueRecord in inventory.ItemList)
                    {
                        itemValueRecord.Date = DateTime.Today.ToString("yyyy-MM-dd");

                        string insertRecord = $"INSERT INTO shopdb.product(id, name, unit, unitprice, quantity, inventoryid, date)VALUES({productId}, '{itemValueRecord.Item.Name}', '{itemValueRecord.Item.Unit.ToString()}',  {itemValueRecord.UnitPrice}, {itemValueRecord.Quantity}, {inventory.Id}, '{itemValueRecord.Date}');";
                        connection.Execute(insertRecord);

                        productId++;
                    }

                    MessageBox.Show("Inventory successfuly added to database!");
                }
                else
                {
                    MessageBox.Show("Please select a store");
                }
            }


        }

        private void dgridStoresInCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AssignInventory(InventoryItems);
        }





        //private Inventory GetInventory(string storename)
        //{
        //    Inventory inventory = new Inventory();

        //    //Finish this method and copy it to the console app
        //    //return inventory;

        //}

    }
}
