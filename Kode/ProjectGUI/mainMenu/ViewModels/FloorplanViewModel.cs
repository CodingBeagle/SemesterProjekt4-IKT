using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DatabaseAPI;
using DatabaseAPI.Factories;
using Microsoft.Win32;
using MvvmFoundation.Wpf;
using mainMenu;

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
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedFileName;

        public string SelectedFileName
        {
            get { return _selectedFileName;}
            set
            {
                if (_selectedFileName != value)
                {
                    _selectedFileName = value;
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
            ImagePath = "../../images/floorplan.jpg";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BrowseFloorplanHandler()
        {
            OpenFileDialog floorplanBrowser = new OpenFileDialog();

            floorplanBrowser.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            bool? browseResult = floorplanBrowser.ShowDialog();

            if (!browseResult.HasValue || !browseResult.Value)
                return;

            SelectedFileName = floorplanBrowser.FileName;
            

        }

        private void UpdateFloorplanHandler()
        {
            DatabaseService dbService = new DatabaseService(new SqlStoreDatabaseFactory());

            dbService.TableFloorplan.UploadFloorplan("floorplan", 10, 10, SelectedFileName);

            ImagePath = null;
            _databaseService.TableFloorplan.DownloadFloorplan();
            var uriSource = new Uri(@"/mainMenu;component../../images/floorplan.jpg", UriKind.Relative);
            ImagePath = "../../images/floorplan.jpg";

            dbService.TableStoreSection.DeleteAllStoreSections(1);
        }
    }
}
