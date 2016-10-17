using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DatabaseAPI.DatabaseModel;

namespace mainMenu.Models
{
    public class DisplayItems : ObservableCollection<DisplayItem>
    {
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
            
        }

        public void Populate(List<Item> searchList)
        {
            foreach (var item in searchList)
            {
                DisplayItem displayItem = new DisplayItem(item);
                Add(displayItem);
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
