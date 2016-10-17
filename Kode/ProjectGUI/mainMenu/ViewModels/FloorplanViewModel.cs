using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DatabaseAPI;
using DatabaseAPI.Factories;
using MvvmFoundation.Wpf;

namespace mainMenu.ViewModels
{
    public class FloorplanViewModel : INotifyPropertyChanged
    {
        private BitmapImage _imagePath;

        public BitmapImage ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
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

        private DatabaseService _databaseService = new DatabaseService(new SqlStoreDatabaseFactory());
         
        public event PropertyChangedEventHandler PropertyChanged;

        public FloorplanViewModel()
        {
            _databaseService.TableFloorplan.DownloadFloorplan();
            var uriSource = new Uri(@"/mainMenu;component../../images/floorplan.jpg", UriKind.Relative);
            ImagePath = new BitmapImage(uriSource);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BrowseFloorplanHandler()
        {
            
        }

        private void UpdateFloorplanHandler()
        {
            
        }
    }
}
