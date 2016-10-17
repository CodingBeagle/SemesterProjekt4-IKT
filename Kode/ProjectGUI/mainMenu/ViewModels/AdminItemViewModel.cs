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
    public class AdminItemViewModel : INotifyPropertyChanged
    {
        public ICommand CreateItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand EditItemCommand { get; set; }
        public ICommand SearchCommand { get; private set; }
        private DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());

        private int _comboBoxIndex;
        private string _searchString;
        private string _itemName;

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
            get { return db.TableItemGroup.GetAllItemGroups(); }
        }

        public AdminItemViewModel()
        {
            try
            {
                ListOfItems = new DisplayItems();
                ComboBoxIndex = -1;
                bool dummybool = false;
                DeleteItemCommand = new RelayCommand(DeleteItem, () => ListOfItems.CurrentIndex >= 0);
                CreateItemCommand = new RelayCommand(AddItem, () => dummybool == false);
                EditItemCommand = new RelayCommand(() => MessageBox.Show("Not Implemented"), () => dummybool == true);
                SearchCommand = new RelayCommand(Search, () => dummybool == false);

                ListOfItems.Populate(db.TableItem.SearchItems(""));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Something went horribly wrong: " + e.Message);
            }
            
        }

        public void DeleteItem()
        {
            try
            {
                DisplayItem selectedItem = ListOfItems[ListOfItems.CurrentIndex];
                db.TableItem.DeleteItem((long)selectedItem.ID);
                ListOfItems.RemoveAt(ListOfItems.CurrentIndex);
                MessageBox.Show($"{selectedItem.VareNavn} blev slettet fra databasen");

            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }

        public void AddItem()
        {
            try
            {
                if (Regex.IsMatch(ItemName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
                {
                    var itemID = db.TableItem.CreateItem(ItemName, ItemGroupComboBoxList[ComboBoxIndex].ItemGroupID);
                    var createdItem = new Item(itemID, ItemName, ItemGroupComboBoxList[ComboBoxIndex].ItemGroupID);
                    ListOfItems.Add(new DisplayItem(createdItem));
                    MessageBox.Show($"{ItemName} er blevet tilføjet til databasen");
                }
                else
                {
                    MessageBox.Show("Navnet på en vare kan kun indeholde bogstaver og tal");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }

        public void Search()
        {
            try
            {
                var searchList = db.TableItem.SearchItems(SearchString);
                ListOfItems.Clear();
                ListOfItems.Populate(searchList);

                if (searchList.Count == 0)
                    MessageBox.Show($"Fandt ingen varer med navnet {SearchString}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}