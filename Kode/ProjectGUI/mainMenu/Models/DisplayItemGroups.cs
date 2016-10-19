using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace mainMenu.Models
{
    public class DisplayItemGroups : ObservableCollection<ItemGroup>
    {
        DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
        private int _currentIndex;

        public int CurrentIndex
        {
            get { return _currentIndex;}
            set
            {
                if (_currentIndex != value)
                {
                    _currentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DisplayItemGroups()
        {
            CurrentIndex = -1;
        }

        public void Populate(string itemGroupName)
        {
            var searchResults = db.TableItemGroup.SearchItemGroups(itemGroupName);
            foreach (var searchResult in searchResults)
            {
                Add(searchResult);
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
}