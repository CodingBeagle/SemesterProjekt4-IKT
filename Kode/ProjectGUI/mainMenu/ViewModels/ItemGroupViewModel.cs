﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.Models;
using MvvmFoundation.Wpf;

namespace mainMenu.ViewModels
{
    public class ItemGroupViewModel : INotifyPropertyChanged
    {
        #region Privates
        private IDatabaseService _db;
        private IMessageBox _messageBox;
        private string _searchString;
        private int _comboBoxIndex;
        private string _itemGroupName;
        private string _previousItemGroupName;
        #endregion

        #region Properties
        public DisplayItemGroups ListOfItemGroups { get; set; }
        public DisplayItemGroups ComboBoxOptions { get; set; }
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
        public int ComboBoxIndex
        {
            get { return _comboBoxIndex; }
            set
            {
                if (_comboBoxIndex != value)
                {
                    _comboBoxIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string ItemGroupName
        {
            get { return _itemGroupName; }
            set
            {
                if (value != _itemGroupName)
                {
                    _itemGroupName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string PreviousItemGroupName
        {
            get { return _previousItemGroupName; }
            set
            {
                if (_previousItemGroupName != value)
                {
                    _previousItemGroupName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region ICommands
        public ICommand CreateItemGroupCommand { get; private set; }
        public ICommand DeleteItemGroupCommand { get; private set; }
        public ICommand EditItemGroupCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        #endregion


        public ItemGroupViewModel(DatabaseService db, IMessageBox mb)
        {
            _db = db;
            _messageBox = mb;
            ComboBoxIndex = -1;
            ItemGroupName = "";
            ListOfItemGroups = new DisplayItemGroups();
            ListOfItemGroups.Populate("");
            ComboBoxOptions = ListOfItemGroups;

            var dummyBool = true;
            CreateItemGroupCommand = new RelayCommand(createItemGroupHandler, () => dummyBool == true);
            DeleteItemGroupCommand = new RelayCommand(deleteItemGroupHandler, () => ListOfItemGroups.CurrentIndex >= 0);
            EditItemGroupCommand = new RelayCommand(editItemGroupHandler,
                () => dummyBool == true);
            SearchCommand = new RelayCommand(searchItemGroupHandler, () => dummyBool == true);


        }

        private void editItemGroupHandler()
        {
            try
            {
                _db.TableItemGroup.UpdateItemGroup(PreviousItemGroupName, ItemGroupName);
                ItemGroup temp = new ItemGroup(ItemGroupName, ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupParentID, ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupID);
                ListOfItemGroups.RemoveAt(ListOfItemGroups.CurrentIndex);
                ListOfItemGroups.Add(temp);
                _messageBox.OpenMessageBox($"Varegruppens navn er blevet opdateret til {ItemGroupName}");
                ItemGroupName = "";
            }
            catch (Exception e)
            {
                _messageBox.OpenMessageBox("Noget gik galt! Check debug for fejlmeddelelse");
                Debug.WriteLine(e.Message);
            }    
        }


        private void createItemGroupHandler()
        {
            long itemGroupID;
            long parentItemGroupID;
            try
            {
                //Check om de indtastede tegn er gyldige. Er vigtigt i forhold til SQL-sætninger   
                if (Regex.IsMatch(ItemGroupName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
                {
                    //Checker om der er valgt en parentItemGroup
                    if (ComboBoxOptions.CurrentIndex == -1)
                    {
                        itemGroupID = _db.TableItemGroup.CreateItemGroup(ItemGroupName);
                        parentItemGroupID = 0;
                    }
                    else
                    {
                        parentItemGroupID = ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupID;
                        itemGroupID = _db.TableItemGroup.CreateItemGroup(ItemGroupName,parentItemGroupID);
                    }
                    
                    ListOfItemGroups.Add(new ItemGroup(ItemGroupName, parentItemGroupID, itemGroupID));
                    _messageBox.OpenMessageBox($"{ItemGroupName} er blevet tilføjet til databasen");
                    ComboBoxIndex = -1;
                    ItemGroupName = "";
                }
                else
                {
                    _messageBox.OpenMessageBox("Navnet på en vare kan kun indeholde bogstaver og tal");
                }

            }
            catch (Exception exception)
            {
                _messageBox.OpenMessageBox("Noget gik galt! Check debug for fejlmeddelelse");
                Debug.WriteLine(exception.Message);
            }
        }

        private void searchItemGroupHandler()
        {
            try
            {
                ListOfItemGroups.Clear();
                ListOfItemGroups.Populate(SearchString);
                

                if (ListOfItemGroups.Count == 0)
                {
                    _messageBox.OpenMessageBox($"Kunne ikke finde nogen varegruppe med navnet {SearchString}");
                    SearchString = "";
                }
                SearchString = "";
            }
            catch (Exception exception)
            {
                _messageBox.OpenMessageBox("Noget gik galt! Check debug for fejlmeddelelse");
                Debug.WriteLine(exception.Message);
            }

        }

        private void deleteItemGroupHandler()
        {

            ItemGroup selectedItem = null;
            try
            {

                selectedItem = ListOfItemGroups[ListOfItemGroups.CurrentIndex];
                _db.TableItemGroup.DeleteItemGroup((long) selectedItem.ItemGroupID);
                ListOfItemGroups.RemoveAt(ListOfItemGroups.CurrentIndex);
                _messageBox.OpenMessageBox($"{selectedItem.ItemGroupName} blev slettet fra databasen");
            }
            catch (SqlException e)
            {
                if (selectedItem != null)
                {
                    _messageBox.OpenMessageBox("Varegruppen " + selectedItem.ItemGroupName +
                                " kan ikke slettes, da denne enten er en over-varegruppe eller den har tilknyttede varer");
                }
                
            }
            catch (Exception exception)
            {
                _messageBox.OpenMessageBox("Noget gik galt! Check debug for fejlmeddelelse");
                Debug.WriteLine(exception.Message);
            }
        }

        public void UpdateItemGroupName()
        {
            PreviousItemGroupName = ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
