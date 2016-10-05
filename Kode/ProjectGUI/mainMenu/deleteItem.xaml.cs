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
    /// Interaction logic for deleteItem.xaml
    /// </summary>
    public partial class deleteItem : Window
    {
        private List<Item> searchList;
        private DatabaseService db;

        public deleteItem()
        {
            InitializeComponent();

            try
            {
                db = new DatabaseService(new SqlDatabaseFactory());
                
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went horribly wrong: {e.Message}");
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            adminItems adminitems = new adminItems();
            adminitems.Show();
            Close();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Item selectedItem = (Item) searchResults.SelectedItem;
                db.TableItem.DeleteItem((long) selectedItem.ItemID);
                searchResults.ItemsSource = searchList;
                MessageBox.Show($"Deleted {selectedItem.Name} from the database");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                searchList = db.TableItem.SearchItems(searchBox.Text);
                searchResults.ItemsSource = searchList;

                if (searchList.Count == 0)
                    MessageBox.Show($"Fandt ingen varer med navnet {searchBox.Text}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
            
        }

        public class DisplayItems
        {
            private Item _item;
            public long ID
            {
                get { return _item.ItemID; }
                set { _item.ItemID = value; }
            }

            public string Name
            {
                get { return _item.Name; }
                set { _item.Name = value; }
            }
        }
    }
}
