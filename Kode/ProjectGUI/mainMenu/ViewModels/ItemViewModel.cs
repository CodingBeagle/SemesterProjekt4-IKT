using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.Models;
using MvvmFoundation.Wpf;

namespace mainMenu.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        #region ICommands
        public ICommand CreateItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand EditItemCommand { get; set; }
        public ICommand SearchItemCommand { get; private set; }
        #endregion
        #region Privates
        private DatabaseService _db = new DatabaseService(new SqlStoreDatabaseFactory());
        private int _comboBoxIndex;
        private string _searchString;
        private string _itemName;
        #endregion
        #region Properties
        public int ComboBoxIndex
        {
            get { return _comboBoxIndex; }

            set
            {
                if (value != this._comboBoxIndex)
                {
                    _comboBoxIndex = value;
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
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (_itemName != value)
                {
                    _itemName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DisplayItems ListOfItems { get; set; }
        public List<ItemGroup> ItemGroupComboBoxList
        {
            get { return _db.TableItemGroup.GetAllItemGroups(); }
        }
        #endregion



        public ItemViewModel()
        {
            try
            {
                ListOfItems = new DisplayItems();
                ComboBoxIndex = -1;
                bool dummybool = false;
                DeleteItemCommand = new RelayCommand(deleteItemHandler, () => ListOfItems.CurrentIndex >= 0);
                CreateItemCommand = new RelayCommand(addItemHandler, () => dummybool == false);
                EditItemCommand = new RelayCommand(() => MessageBox.Show("Not Implemented"), () => dummybool == true);
                SearchItemCommand = new RelayCommand(searchItemHandler, () => dummybool == false);

                ListOfItems.Populate(_db.TableItem.SearchItems(""));
            }
            catch (Exception e)
            {
                MessageBox.Show("Noget gik galt! Check Debug for fejlmeddelelsel");
                Debug.WriteLine(e.Message);
            }
            
        }

        private void deleteItemHandler()
        {
            try
            {
                DisplayItem selectedItem = ListOfItems[ListOfItems.CurrentIndex];
                _db.TableItem.DeleteItem((long)selectedItem.ID);
                ListOfItems.RemoveAt(ListOfItems.CurrentIndex);
                MessageBox.Show($"{selectedItem.VareNavn} blev slettet fra databasen");

            }
            catch (Exception exception)
            {
                MessageBox.Show("Noget gik galt! Check Debug for fejlmeddelelsel");
                Debug.WriteLine(exception.Message); 
            }
        }

        private void addItemHandler()
        {
            try
            {
                if (Regex.IsMatch(ItemName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
                {
                    var itemID = _db.TableItem.CreateItem(ItemName, ItemGroupComboBoxList[ComboBoxIndex].ItemGroupID);
                    var createdItem = new Item(itemID, ItemName, ItemGroupComboBoxList[ComboBoxIndex].ItemGroupID);
                    ListOfItems.Add(new DisplayItem(createdItem));
                    MessageBox.Show($"{ItemName} er blevet tilføjet til databasen");
                    ItemName = "";
                    ComboBoxIndex = -1;
                }
                else
                {
                    MessageBox.Show("Navnet på en vare kan kun indeholde bogstaver og tal");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Noget gik galt! Check Debug for fejlmeddelelsel");
                Debug.WriteLine(exception.Message);
            }
        }

        private void searchItemHandler()
        {
            ItemUtility.SearchItem(_db,SearchString,ListOfItems);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}