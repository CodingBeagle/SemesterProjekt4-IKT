using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AddItemDialog.xaml
    /// </summary>
    public partial class AddItemDialog : Window
    {
        public List<ItemGroup> WareGroups;
        private DatabaseService db;
        private DisplayItems _displayItemses;
        public AddItemDialog(DisplayItems displayItemses)
        {
            InitializeComponent();
            try
            {
                db = new DatabaseService(new SqlStoreDatabaseFactory());
                //WareGroups = db.TableItemGroup.GetAllItemGroups();
                ItemGroupComboBox.ItemsSource = displayItemses.cbItemGroups;
                ItemGroupComboBox.DisplayMemberPath = "ItemGroupName";
                _displayItemses = displayItemses;
                ItemNameTextBox.DataContext = displayItemses;
                CreateItemButton.DataContext = displayItemses;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Der er noget der gik galt: {e.Message}");
            }
        }

        private void CreateItemButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var itemName = ItemNameTextBox.Text;
            //    var group = (ItemGroup)ItemGroupComboBox.SelectedItem;
            //    if (Regex.IsMatch(itemName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
            //    {
            //        var itemID = db.TableItem.CreateItem(itemName, @group.ItemGroupID);
            //        var createdItem = new Item(itemID, itemName, @group.ItemGroupID);
            //        _displayItemses.Add(new DisplayItem(createdItem));
            //        MessageBox.Show($"{itemName} er blevet tilføjet til databasen til varegruppen {group.ItemGroupName}");
            //        Close();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Navnet på en vare kan kun indeholde bogstaver og tal");
            //    }

            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            //}
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
