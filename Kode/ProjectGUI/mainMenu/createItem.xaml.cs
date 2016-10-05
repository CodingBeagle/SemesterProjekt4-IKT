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
using System.Windows.Shapes;
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
        private DatabaseService db  = new DatabaseService(new SqlDatabaseFactory());
        public createItem()
        {
            InitializeComponent();

            wareGroups = db.TableItemGroup.GetAllItemGroups();
            groupBox.ItemsSource = wareGroups;

        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            adminItems adminitems = new adminItems();
            adminitems.Show();
            Close();
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            string itemName = itemNameBox.Text;
            ItemGroup group = (ItemGroup)groupBox.SelectedItem;
            db.TableItem.CreateItem(itemName, (long)@group.ItemGroupID);

            MessageBox.Show($"{itemName} er blevet tilføjet til databsen til varegruppen {group.ItemGroupName}");
            groupBox.SelectedIndex = 0;
            itemNameBox.Text = "";

        }
    }
}
