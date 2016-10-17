using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;

namespace mainMenu.ViewModels
{
    public class FloorplanViewModel : INotifyPropertyChanged
    {
        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (value != _imagePath)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _backCommand;

        public ICommand BackCommand
        {
            get { return _backCommand ?? (_backCommand = new RelayCommand(BackHandler)); }
        }

        private ICommand _browseFloorplanCommand;

        public ICommand BrowseFloorplanCommand
        {
            get
            {
                return _browseFloorplanCommand ?? (_browseFloorplanCommand = new RelayCommand(BrowseFloorplanHandler));
            }
        }

        private ICommand _updateFloorplanCommand;

        public ICommand UpdateFloorplanCommand
        {
            get
            {
                return _updateFloorplanCommand ?? (_updateFloorplanCommand = new RelayCommand(UpdateFloorplanHandler));
            }
        }

        private Window _currentWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        public FloorplanViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackHandler()
        {
            
        }

        private void BrowseFloorplanHandler()
        {
            
        }

        private void UpdateFloorplanHandler()
        {
            
        }
    }
}
