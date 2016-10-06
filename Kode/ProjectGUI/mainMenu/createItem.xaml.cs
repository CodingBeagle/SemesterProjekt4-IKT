using System;
using System.Collections.Generic;
using System.Windows;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for createItem.xaml
    /// </summary>
    public partial class createItem : Window
    {
        public List<ItemGroup> wareGroups;
        private DatabaseService db;

        public createItem()
        {
            InitializeComponent();
            try
            {
                db = new DatabaseService(new SqlStoreDatabaseFactory());
                wareGroups = db.TableItemGroup.GetAllItemGroups();
                groupBox.ItemsSource = wareGroups;

            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
            
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            adminItems adminitems = new adminItems();
            adminitems.Show();
            Close();
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string itemName = itemNameBox.Text;
                ItemGroup group = (ItemGroup) groupBox.SelectedItem;
                db.TableItem.CreateItem(itemName, (long) @group.ItemGroupID);

                MessageBox.Show($"{itemName} er blevet tilføjet til databasen til varegruppen {group.ItemGroupName}");
                groupBox.SelectedIndex = 0;
                itemNameBox.Text = "";
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
            

        }
    }
}
