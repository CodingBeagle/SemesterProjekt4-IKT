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


        protected override event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}