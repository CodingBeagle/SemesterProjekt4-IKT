﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
using mainMenu.ViewModels;
using MvvmFoundation.Wpf;

namespace mainMenu
{
    public class StoreSectionViewModel : INotifyPropertyChanged
    {
        #region ICommand
        public ICommand WindowLoadedCommand { get; private set; }
        public ICommand SelectCurrentStoreSectionCommand { get; private set; }
        public ICommand DeleteStoreSectionCommand { get; private set; }
        public ICommand EditStoreSectionCommand { get; private set; }
        public ICommand SearchItemsCommand { get; private set; }
        public ICommand AddItemToSectionCommand { get; private set; }
        public ICommand RemoveItemFromSectionCommand { get; private set; }
        public ICommand AddStoreSectionCommand { get; private set; }
        public ICommand CancelStoreSectionCommand { get; private set; }
        public ICommand EditStoreSectionDialogOKCommand { get; private set; }
        #endregion

        #region Privates
        private IDatabaseService _db;
        private long _floorplanID = 1;
        private List<StoreSection> _storeSectionList;
        private string _searchString;
        private long _selectedStoreSection = 0;
        private IList _selectedItemsList = new ArrayList();
        private List<Item> _itemsInSectionList = new List<Item>();
        private string _selectedStoreSectionName;
        private ImageBrush _floorplanImage;
        private string _newlyCreatedStoreSectionName;
        private SectionShape _newlyCreatedSection;
        private StoreSection _sectionToEdit;
        private IMessageBox _messageBox;
        #endregion

        #region Properties

        public string PreviousSectionName { get; set; }
        public string NewSectionName { get; set; }

        public DisplayItems ListOfItems { get; set; }
        public Item SelectedSectionItem { get; set; }
        public ObservableCollection<SectionShape> ShapeCollection { get; set; }
        public string NewlyCreatedStoreSectionName
        {
            get { return _newlyCreatedStoreSectionName; }
            set
            {
                if (_newlyCreatedStoreSectionName != value)
                {
                    _newlyCreatedStoreSectionName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (_searchString != value)
                {
                    _searchString = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public IList SelectedItemsList
        {
            get { return _selectedItemsList; }
            set { _selectedItemsList = value; NotifyPropertyChanged(); }
        }
        public List<Item> ItemsInSectionList
        {
            get { return _itemsInSectionList; }
            set
            {
                if (value != _itemsInSectionList)
                {
                    _itemsInSectionList = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SelectedStoreSectionName
        {
            get { return _selectedStoreSectionName; }
            set
            {
                if (_selectedStoreSectionName != value)
                {
                    _selectedStoreSectionName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ImageBrush FloorplanImage
        {
            get { return _floorplanImage; }
            set
            {
                if (_floorplanImage != value)
                {
                    _floorplanImage = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        public StoreSectionViewModel(DatabaseService database, IMessageBox mb)
        {
            try
            {
                _db = database;
                _messageBox = mb;
            }
            catch (Exception e)
            {
                _messageBox.OpenMessageBox("Noget gik galt! Check Debug for fejlmeddelelse");
                Debug.WriteLine(e.Message);
            }
            ShapeCollection = new ObservableCollection<SectionShape>();
            ListOfItems = new DisplayItems();

            WindowLoadedCommand = new RelayCommand(windowLoadedHandler);
            SelectCurrentStoreSectionCommand = new RelayCommand<SectionShape>(selectCurrentStoreSectionHandler);
            DeleteStoreSectionCommand = new RelayCommand(deleteStoreSectionHandler, () => _selectedStoreSection != 0);
            EditStoreSectionCommand = new RelayCommand(editStoreSectionHandler, () => _selectedStoreSection != 0);
            SearchItemsCommand = new RelayCommand(searchItemsHandler);
            AddItemToSectionCommand = new RelayCommand(addItemToSectionHandler, () => _selectedStoreSection != 0);
            RemoveItemFromSectionCommand = new RelayCommand(removeItemFromSectionHandler, () => _selectedStoreSection != 0);
            AddStoreSectionCommand = new RelayCommand(AddStoreSectionDialogOkButtonHandler, () => CheckValidName(NewlyCreatedStoreSectionName));
            CancelStoreSectionCommand = new RelayCommand(AddStoreSectionDialogCancelButtonHandler);
            EditStoreSectionDialogOKCommand = new RelayCommand(editStoreSectionButtonOKHandler, () => CheckValidName(NewSectionName));
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
            _db.TableFloorplan.DownloadFloorplan(@"../../images/");

            FloorplanImage = null;
            ImageBrush floorplanImgBrush = new ImageBrush();
            RefreshableImage refresh = new RefreshableImage();
            BitmapImage result = refresh.Get("../../images/floorplan.jpg");

            floorplanImgBrush.ImageSource = result;
            FloorplanImage = floorplanImgBrush;
        }

        public void CreateStoreSection(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            SectionShape newSectionShape = new SectionShape();

            newSectionShape.Top = e.GetPosition(canvas).Y-7;
            newSectionShape.Left = e.GetPosition(canvas).X-7;

            newSectionShape.Shape = ShapeButtonCreator.CreateShapeForButton();
            
            ShapeCollection.Add(newSectionShape);
            _newlyCreatedSection = newSectionShape;

            NewlyCreatedStoreSectionName = "";
        }

        public void AddStoreSectionDialogOkButtonHandler()
        {
            if (Regex.IsMatch(NewlyCreatedStoreSectionName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
            {
                long newStoreSectionID = _db.TableStoreSection.CreateStoreSection(NewlyCreatedStoreSectionName,
                    (long)_newlyCreatedSection.Left, (long)_newlyCreatedSection.Top, _floorplanID);
                _newlyCreatedSection.ID = newStoreSectionID;
                _newlyCreatedSection.Name = "Button" + newStoreSectionID;
            }
            else
            {
                ShapeCollection.Remove(_newlyCreatedSection);
            }
        }

        private bool CheckValidName(string Name)
        {
            if (Regex.IsMatch(Name, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddStoreSectionDialogCancelButtonHandler()
        {
            ShapeCollection.Remove(_newlyCreatedSection);
        }

        private void selectCurrentStoreSectionHandler(SectionShape shape)
        {
            _selectedStoreSection = shape.ID;
            ItemsInSectionList = _db.TableItemSectionPlacement.ListItemsInSection(_selectedStoreSection);

            StoreSection selectedStoreSection = _db.TableStoreSection.GetStoreSection(_selectedStoreSection);
            SelectedStoreSectionName = selectedStoreSection.Name;
           
        }

        private void deleteStoreSectionHandler()
        {
            SectionShape shapeToDelete = null;
            string sectionShapeToDelete = "Button" + _selectedStoreSection;

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

            _db.TableStoreSection.DeleteStoreSection(_selectedStoreSection);
        }

        private void editStoreSectionHandler()
        {
           //THIS IS A DUMMY NOW
        }

        public void PrepareSectionToEdit()
        {
            _sectionToEdit = _db.TableStoreSection.GetStoreSection(_selectedStoreSection);
            NewSectionName = "";
            PreviousSectionName = _sectionToEdit.Name;
        }

        private void editStoreSectionButtonOKHandler()
        {
            if (Regex.IsMatch(NewSectionName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
            {
                _db.TableStoreSection.UpdateStoreSectionName(_selectedStoreSection, NewSectionName);
                _sectionToEdit.Name = NewSectionName;
                SelectedStoreSectionName = _sectionToEdit.Name;
            }
        }
        private void searchItemsHandler()
        {
            ItemUtility.SearchItem(_db, SearchString, ListOfItems, _messageBox);
        }

        private void addItemToSectionHandler()
        {
            foreach (DisplayItem item in SelectedItemsList)
            {
                int findValue = ItemsInSectionList.FindIndex(currentItem => currentItem.ItemID == item.ID);

                if (findValue == -1)
                {
                    _db.TableItemSectionPlacement.PlaceItem(item.ID, _selectedStoreSection);
                }
                else
                {
                    _messageBox.OpenMessageBox("Varen " + item.VareNavn + " findes i sektionen i forvejen");
                }               
                
            }

            ItemsInSectionList = _db.TableItemSectionPlacement.ListItemsInSection(_selectedStoreSection);
        }

        private void removeItemFromSectionHandler()
        {
            _db.TableItemSectionPlacement.DeletePlacement(SelectedSectionItem.ItemID,_selectedStoreSection);
            ItemsInSectionList = _db.TableItemSectionPlacement.ListItemsInSection(_selectedStoreSection);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
