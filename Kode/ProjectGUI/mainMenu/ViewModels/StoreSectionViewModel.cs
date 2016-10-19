using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.FloorplanLogic;
using mainMenu.Models;
using mainMenu.RefreshLogic;
using MvvmFoundation.Wpf;

namespace mainMenu
{
    public class StoreSectionViewModel : INotifyPropertyChanged
    { 
        public ICommand WindowLoadedCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand SelectCurrentStoreSectionCommand { get; private set; }
        public ICommand DeleteStoreSectionCommand { get; private set; }
        public ICommand EditStoreSectionCommand { get; private set; }
        public ICommand SearchItemsCommand { get; private set; }
        public ICommand AddItemToSectionCommand { get; private set; }
        public ICommand RemoveItemFromSectionCommand { get; private set; }


        public string NewSectionName { get; set; }
        public  string PreviousSectionName { get; set; }
        public DisplayItems ListOfItems { get; set; }
        public Item SelectedSectionItem { get; set; }
        public ObservableCollection<SectionShape> ShapeCollection { get; set; }
        public long SelectedStoreSection = 0;

        private Window _currentWindow { get; }
        private DatabaseService _db;
        private long _floorplanID = 1;
        private List<StoreSection> _storeSectionList;


        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (_searchString != value)
                {
                    _searchString = value;
                    OnPropertyChanged();
                }
            }
        }

        private IList _selectedItemsList = new ArrayList();
        public IList SelectedItemsList
        {
            get {return _selectedItemsList;}
            set { _selectedItemsList = value; OnPropertyChanged(); }
        }

        private List<Item> _itemsInSectionList = new List<Item>();
        public List<Item> ItemsInSectionList
        {
            get { return _itemsInSectionList; }
            set
            {
                if (value != _itemsInSectionList)
                {
                    _itemsInSectionList = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedStoreSectionName;
        public string SelectedStoreSectionName
        {
            get { return _selectedStoreSectionName; }
            set
            {
                if (_selectedStoreSectionName != value)
                {
                    _selectedStoreSectionName = value;
                    OnPropertyChanged();
                }
            }
        }

        private ImageBrush _floorplanImage;
        public ImageBrush FloorplanImage
        {
            get { return _floorplanImage; }
            set
            {
                if (_floorplanImage != value)
                {
                    _floorplanImage = value;
                    OnPropertyChanged();
                }
            }
        }

        public StoreSectionViewModel(Window window)
        {
            try
            {
                _db = new DatabaseService(new SqlStoreDatabaseFactory());
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong" + e.Message);
            }
            ShapeCollection = new ObservableCollection<SectionShape>();
            ListOfItems = new DisplayItems();
            _currentWindow = window;

            WindowLoadedCommand = new RelayCommand(windowLoadedHandler);
            BackCommand = new RelayCommand(backHandler);
            SelectCurrentStoreSectionCommand = new RelayCommand<SectionShape>(selectCurrentStoreSectionHandler);
            DeleteStoreSectionCommand = new RelayCommand(deleteStoreSectionHandler, () => SelectedStoreSection != 0);
            EditStoreSectionCommand = new RelayCommand(editStoreSectionHandler, () => SelectedStoreSection != 0);
            SearchItemsCommand = new RelayCommand(searchItemsHandler);
            AddItemToSectionCommand = new RelayCommand(addItemToSectionHandler, () => SelectedStoreSection != 0);
            RemoveItemFromSectionCommand = new RelayCommand(removeItemFromSectionHandler, () => SelectedStoreSection != 0);
            
        }

        private void windowLoadedHandler()
        {
            // Load sections from database and display on canvas
            _storeSectionList = _db.TableStoreSection.GetAllStoreSections(_floorplanID);
            foreach (var section in _storeSectionList)
            {
                SectionShape loadedSectionShape = new SectionShape();
                loadedSectionShape.Top = section.CoordinateY;
                loadedSectionShape.Left = section.CoordinateX;
                loadedSectionShape.Shape = ShapeButtonCreator.CreateShapeForButton();
                loadedSectionShape.Name = "Button" + section.StoreSectionID;
                loadedSectionShape.ID = section.StoreSectionID;

                ShapeCollection.Add(loadedSectionShape);
            }

            // Load items from database and display on itemdatagrid
            ListOfItems.Populate(_db.TableItem.SearchItems(""));


            // Load floorplan and display on canvas
            _db.TableFloorplan.DownloadFloorplan();

            FloorplanImage = null;
            ImageBrush floorplanImgBrush = new ImageBrush();
            RefreshableImage refresh = new RefreshableImage();
            BitmapImage result = refresh.Get("../../images/floorplan.jpg");

            #region Outcommented Refreshlogic
            /*BitmapImage result = new BitmapImage();
           result.BeginInit();
           result.UriSource = new Uri("../../images/floorplan.jpg", UriKind.Relative);
           // .OnLoad makes sure WPF prevents keeping a lock on the file
           result.CacheOption = BitmapCacheOption.OnLoad;
           // .IgnoreImageCache causes WPF to reread the image every time
           // Should be used when selected images needs to be refreshed
           result.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
           result.EndInit();*/


            #endregion

            floorplanImgBrush.ImageSource = result;
            FloorplanImage = floorplanImgBrush;
        }
        private void backHandler()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            _currentWindow.Close();
        }

        public void CreateStoreSection(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            SectionShape newSectionShape = new SectionShape();

            newSectionShape.Top = e.GetPosition(canvas).Y;
            newSectionShape.Left = e.GetPosition(canvas).X;

            newSectionShape.Shape = ShapeButtonCreator.CreateShapeForButton();
            
            ShapeCollection.Add(newSectionShape);

            AddSectionDialog newSectionDialog = new AddSectionDialog();
            newSectionDialog.ShowDialog();

            if (newSectionDialog.IsOKPressed)
            {
                long newStoreSectionID = _db.TableStoreSection.CreateStoreSection(newSectionDialog.SectionName, (long)newSectionShape.Left, (long)newSectionShape.Top, _floorplanID);
                newSectionShape.ID = newStoreSectionID;
                newSectionShape.Name = "Button" + newStoreSectionID;
            }
            else
            {
                ShapeCollection.Remove(newSectionShape);
            }
        }

        private void selectCurrentStoreSectionHandler(SectionShape shape)
        {
            SelectedStoreSection = shape.ID;
            ItemsInSectionList = _db.TableItemSectionPlacement.ListItemsInSection(SelectedStoreSection);

            StoreSection selectedStoreSection = _db.TableStoreSection.GetStoreSection(SelectedStoreSection);
            SelectedStoreSectionName = selectedStoreSection.Name;
           
        }

   

        private void deleteStoreSectionHandler()
        {
            SectionShape shapeToDelete = null;
            string sectionShapeToDelete = "Button" + SelectedStoreSection;

            foreach (var shape in ShapeCollection)
            {
                if (shape.Name == sectionShapeToDelete)
                {
                    shapeToDelete = shape;
                }
            }

            if (shapeToDelete != null)
            {
                ShapeCollection.Remove(shapeToDelete);
            }

            _db.TableStoreSection.DeleteStoreSection(SelectedStoreSection);
        }

        private void editStoreSectionHandler()
        {
            StoreSection sectionToEdit = _db.TableStoreSection.GetStoreSection(SelectedStoreSection);
            NewSectionName = "";
            PreviousSectionName = sectionToEdit.Name;

            EditSectionDialog newSectionDialog = new EditSectionDialog(this, sectionToEdit.Name);
            newSectionDialog.ShowDialog();

            if (newSectionDialog.IsSectionNameChanged)
            {
                _db.TableStoreSection.UpdateStoreSectionName(SelectedStoreSection, NewSectionName);
            }

        }

        private void searchItemsHandler()
        {
            ItemUtility.SearchItem(_db, SearchString, ListOfItems);
        }

        private void addItemToSectionHandler()
        {
            foreach (DisplayItem item in SelectedItemsList)
            {
                int findValue = ItemsInSectionList.FindIndex(currentItem => currentItem.ItemID == item.ID);

                if (findValue == -1)
                {
                    _db.TableItemSectionPlacement.PlaceItem(item.ID, SelectedStoreSection);
                }
                else
                {
                    MessageBox.Show("Varen " + item.VareNavn + " findes i sektionen i forvejen");
                }               
                
            }

            ItemsInSectionList = _db.TableItemSectionPlacement.ListItemsInSection(SelectedStoreSection);
        }

        private void removeItemFromSectionHandler()
        {
            _db.TableItemSectionPlacement.DeletePlacement(SelectedSectionItem.ItemID,SelectedStoreSection);
            ItemsInSectionList = _db.TableItemSectionPlacement.ListItemsInSection(SelectedStoreSection);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
