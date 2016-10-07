using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private List<DisplayItems> displayItemses = new List<DisplayItems>();
        private DatabaseService db;

        public deleteItem()
        {
            InitializeComponent();
            searchResults.ItemsSource = searchList;
            //searchResults.ItemsSource = displayItemses;

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
            adminItems adminitems = new adminItems();
            adminitems.Show();
            Close();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Item selectedItem = (Item)searchResults.SelectedItem;
                db.TableItem.DeleteItem((long)selectedItem.ItemID);

                //DisplayItems selectedItem = (DisplayItems) searchResults.SelectedItem;
                //db.TableItem.DeleteItem((long) selectedItem.ID);


                searchResults.Items.Refresh();
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
                //foreach (var item in searchList)
                //{
                //    var displayItem = new DisplayItems(item);
                //    displayItemses.Add(displayItem);
                //}
                //searchResults.ItemsSource = null;
                //searchResults.ItemsSource = displayItemses;

                if (searchList.Count == 0)
                    MessageBox.Show($"Fandt ingen varer med navnet {searchBox.Text}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
            
        }

        public class DisplayItems : INotifyPropertyChanged
        {

            // INotifyPropertyChanged Members
            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            private Item _item;
            DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());

            public DisplayItems(Item item)
            {
                
                _item = new Item(item.ItemID, item.Name, item.ItemGroupID);

            }
            public long ID
            {
                get { return _item.ItemID; }
                private set { _item.ItemID = value; NotifyPropertyChanged();}
            }

            public string Name
            {
                get { return _item.Name; }
                private set { _item.Name = value; NotifyPropertyChanged(); }
            }

            public long ItemGroupID
            {
                get { return _item.ItemGroupID; }
                private set { _item.ItemGroupID = value; NotifyPropertyChanged(); }
            }

            public string ItemGroupName
            {
                get
                {
                    ItemGroup itemGroup = db.TableItemGroup.GetItemGroup(ItemGroupID);
                    return itemGroup.ItemGroupName;
                }
                private set { }
                
            }
        }
    }
}
