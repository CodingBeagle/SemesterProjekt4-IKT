using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using MvvmFoundation.Wpf;

namespace mainMenu
{
    public class DisplayItems : ObservableCollection<DisplayItem>
    {
        DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());

        public ICommand DeleteItemCommand { get; private set; }
        public ICommand EditItemCommand { get; private set; }

        public ICommand AddNewItemCommand { get; private set; }

        private int _currentIndex;

        public int CurrentIndex
        {
            get { return _currentIndex; }

            set
            {
                if (value != this._currentIndex)
                {
                    _currentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DisplayItems() : base()
        {
            CurrentIndex = -1;
            bool dummybool = false;
            DeleteItemCommand = new RelayCommand(DeleteItem, () => CurrentIndex >= 0);
            EditItemCommand = new RelayCommand(() => MessageBox.Show("Not Implemented"), () => dummybool == true);
        }

        public void Populate(List<Item> searchList)
        {
            foreach (var item in searchList)
            {
                DisplayItem displayItem = new DisplayItem(item);
                Add(displayItem);
            }
        }

        public void DeleteItem()
        {
            try
            {
                DisplayItem selectedItem = this.Items[CurrentIndex];
                db.TableItem.DeleteItem((long)selectedItem.ID);
                RemoveAt(CurrentIndex);
                MessageBox.Show($"{selectedItem.VareNavn} blev slettet fra databasen");
                
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }


        protected override event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
    public class DisplayItem : INotifyPropertyChanged
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

        public DisplayItem(Item item)
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
