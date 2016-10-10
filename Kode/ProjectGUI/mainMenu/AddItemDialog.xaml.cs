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
        public List<ItemGroup> wareGroups;
        private DatabaseService db;
        public AddItemDialog()
        {
            InitializeComponent();
            try
            {
                db = new DatabaseService(new SqlStoreDatabaseFactory());
                wareGroups = db.TableItemGroup.GetAllItemGroups();
                ItemGroupComboBox.ItemsSource = wareGroups;
                ItemGroupComboBox.DisplayMemberPath = "ItemGroupName";

            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went horribly wrong: {e.Message}");
            }
        }

        private void CreateItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string itemName = ItemNameTextBox.Text;
                if (Regex.IsMatch(itemName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
                {
                    ItemGroup group = (ItemGroup)ItemGroupComboBox.SelectedItem;
                    db.TableItem.CreateItem(itemName, (long)@group.ItemGroupID);

                    MessageBox.Show($"{itemName} er blevet tilføjet til databasen til varegruppen {group.ItemGroupName}");
                    Close();
                }
                else
                {
                    MessageBox.Show("Navnet på en vare kan kun indeholde bogstaver og tal");
                }
                
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
