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
    /// Interaction logic for adminItems.xaml
    /// </summary>
    public partial class adminItems : Window
    {
        private List<Item> searchList;
        private List<deleteItem.DisplayItems> displayItemses = new List<deleteItem.DisplayItems>();
        private DatabaseService db;

        public adminItems()
        {
            InitializeComponent();
            SearchResultGrid.ItemsSource = searchList;

            try
            {
                db = new DatabaseService(new SqlStoreDatabaseFactory());

            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went horribly wrong: {e.Message}");
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
            
            
        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            AddItemDialog newAddItemDialog = new AddItemDialog();
            newAddItemDialog.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                searchList = db.TableItem.SearchItems(SearchBox.Text);
                SearchResultGrid.ItemsSource = searchList;
                //foreach (var item in searchList)
                //{
                //    var displayItem = new DisplayItems(item);
                //    displayItemses.Add(displayItem);
                //}
                //searchResults.ItemsSource = null;
                //searchResults.ItemsSource = displayItemses;

                if (searchList.Count == 0)
                    MessageBox.Show($"Fandt ingen varer med navnet {SearchBox.Text}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }

        private void DeleteItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Item selectedItem = (Item)SearchResultGrid.SelectedItem;
                db.TableItem.DeleteItem((long)selectedItem.ItemID);

                //DisplayItems selectedItem = (DisplayItems) searchResults.SelectedItem;
                //db.TableItem.DeleteItem((long) selectedItem.ID);

                MessageBox.Show($"Deleted {selectedItem.Name} from the database");
                searchList = db.TableItem.SearchItems(SearchBox.Text);
                SearchResultGrid.ItemsSource = searchList;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }
    }
}
