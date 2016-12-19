using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mainMenu
{
    class StoreSectionViewModel: INotifyPropertyChanged
    {
        public ICommand BackCommand { get; private set; }
        public ICommand CreateStoreSectionCommand { get; private set; }
        public ICommand DeleteStoreSectionCommand { get; private set; }
        public ICommand EditStoreSectionCommand { get; private set; }
        public ICommand OpenCreateStoreSectionWindowCommand { get; private set; }
        public ICommand OpenEditStoreSectionWindowCommand { get; private set; }
        public string NewSectionName { get; private set; }

        private void backHandler()
        {
            
        }

        private void createStoreSectionHandler()
        {
            
        }

        private void deleteStoreSectionHandler()
        {
            
        }

        private void editStoreSectionHandler()
        {
            
        }

        private void openCreateStoreSectionWindowHandler()
        {
            
        }

        private void openEditStoreSectionWindowHandler()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
