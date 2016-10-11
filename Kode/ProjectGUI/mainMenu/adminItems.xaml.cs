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
    /// Interaction logic for adminItems.xaml
    /// </summary>
    public partial class adminItems : Window
    {
        private List<Item> searchList;
        private List<DisplayItems> displayItemses = new List<DisplayItems>();
        private DatabaseService db;

        public adminItems()
        {
            InitializeComponent();
            //SearchResultGrid.ItemsSource = searchList;
            SearchResultGrid.ItemsSource = displayItemses;

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
                displayItemses.Clear();
                //SearchResultGrid.ItemsSource = searchList;
                foreach (var item in searchList)
                {
                    DisplayItems displayItem = new DisplayItems(item);
                    displayItemses.Add(displayItem);
                }
                SearchResultGrid.ItemsSource = null;
                SearchResultGrid.ItemsSource = displayItemses;

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
                //Item selectedItem = (Item)SearchResultGrid.SelectedItem;
                //db.TableItem.DeleteItem((long)selectedItem.ItemID);

                DisplayItems selectedItem = (DisplayItems)SearchResultGrid.SelectedItem;
                db.TableItem.DeleteItem((long)selectedItem.ID);
                displayItemses.RemoveAt(SearchResultGrid.SelectedIndex);

                MessageBox.Show($"{selectedItem.VareNavn} Blev slettet fra databasen");
                searchList = db.TableItem.SearchItems(SearchBox.Text);
                //SearchResultGrid.ItemsSource = searchList;
                SearchResultGrid.ItemsSource = null;
                SearchResultGrid.ItemsSource = displayItemses;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
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
            private set { _item.ItemID = value; NotifyPropertyChanged(); }
        }

        public string VareNavn //Name
        {
            get { return _item.Name; }
            private set { _item.Name = value; NotifyPropertyChanged(); }
        }

        public long VareGruppeID //ItemGroupID
        {
            get { return _item.ItemGroupID; }
            private set { _item.ItemGroupID = value; NotifyPropertyChanged(); }
        }

        public string VareGruppeNavn //ItemGroupName
        {
            get
            {
                ItemGroup itemGroup = db.TableItemGroup.GetItemGroup(VareGruppeID);
                return itemGroup.ItemGroupName;
            }
            private set { }

        }
    }
}
