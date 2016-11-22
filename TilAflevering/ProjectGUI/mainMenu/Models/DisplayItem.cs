using System.ComponentModel;
using System.Runtime.CompilerServices;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace mainMenu.Models
{
    public class DisplayItem : INotifyPropertyChanged
    {
        DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
        private Item _item;

        public DisplayItem(Item item)
        {

            _item = new Item(item.ItemID, item.Name, item.ItemGroupID);

        }
        public long ID
        {
            get { return _item.ItemID; }
            private set
            {
                if (_item.ItemID != value)
                {
                    _item.ItemID = value;
                    NotifyPropertyChanged();
                }
            }
        }                                   
        public string VareNavn                              
        {
            get { return _item.Name; }
            private set
            {
                if (_item.Name != value)
                {
                    _item.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public long VareGruppeID                               
        {
            get { return _item.ItemGroupID; }
            private set
            {
                if (_item.ItemGroupID != value)
                {
                    _item.ItemGroupID = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string VareGruppeNavn                           
        {
            get
            {
                ItemGroup itemGroup = db.TableItemGroup.GetItemGroup(VareGruppeID);
                return itemGroup.ItemGroupName;
            }
            private set { }

        }


        // INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}