using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DatabaseAPI;
using DatabaseAPI.Factories;
using Microsoft.Win32;
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
                return _browseFloorplanCommand ?? (_browseFloorplanCommand = new RelayCommand(browseFloorplanHandler));
            }
        }

        private ICommand _updateFloorplanCommand;

        public ICommand UpdateFloorplanCommand
        {
            get
            {
                return _updateFloorplanCommand ?? (_updateFloorplanCommand = new RelayCommand(updateFloorplanHandler));
            }
        }

        private readonly DatabaseService _db = new DatabaseService(new SqlStoreDatabaseFactory());
         
        public event PropertyChangedEventHandler PropertyChanged;

        public FloorplanViewModel()
        {
            refreshFloorplanThumbnail();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void browseFloorplanHandler()
        {
            OpenFileDialog floorplanBrowser = new OpenFileDialog();

            floorplanBrowser.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            bool? browseResult = floorplanBrowser.ShowDialog();

            if (!browseResult.HasValue || !browseResult.Value)
                return;

            SelectedFileName = floorplanBrowser.FileName;
        }

        private void updateFloorplanHandler()
        {
            _db.TableFloorplan.UploadFloorplan("floorplan", 10, 10, SelectedFileName);
            refreshFloorplanThumbnail();
            _db.TableStoreSection.DeleteAllStoreSections(1);
        }

        private void refreshFloorplanThumbnail()
        {
            ImagePath = null;
            _db.TableFloorplan.DownloadFloorplan();
            var uriSource = new Uri(@"/mainMenu;component../../images/floorplan.jpg", UriKind.Relative);
            ImagePath = "../../images/floorplan.jpg";
        }
    }
}