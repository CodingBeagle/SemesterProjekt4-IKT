using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class AdminItemGroupsViewModel : INotifyPropertyChanged
    {
        private DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
        public DisplayItemGroups ListOfItemGroups { get; set; }
        public DisplayItemGroups ComboBoxOptions { get; set; }

        #region Privates
        private string _searchString;
        private int _comboBoxIndex;
        private string _itemGroupName;
        private string _oldItemGroupName;



        #endregion

        #region MyRegion
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
        public string OldItemGroupName
        {
            get { return _oldItemGroupName; }
            set
            {
                if (_oldItemGroupName != value)
                {
                    _oldItemGroupName = value;
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


        public AdminItemGroupsViewModel()
        {
            ComboBoxIndex = -1;
            ItemGroupName = "";
            ListOfItemGroups = new DisplayItemGroups();
            ListOfItemGroups.Populate("");
            ComboBoxOptions = ListOfItemGroups;

            var dummyBool = true;
            CreateItemGroupCommand = new RelayCommand(CreateItemGroup, () => dummyBool == true);
            DeleteItemGroupCommand = new RelayCommand(DeleteItemGroup, () => ListOfItemGroups.CurrentIndex >= 0);
            EditItemGroupCommand = new RelayCommand(EditItemGroup,
                () => dummyBool == true);
            SearchCommand = new RelayCommand(Search, () => dummyBool == true);


        }

        public void EditItemGroup()
        {
            try
            {
                db.TableItemGroup.UpdateItemGroup(OldItemGroupName, ItemGroupName);
                ItemGroup temp = new ItemGroup(ItemGroupName, ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupParentID, ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupID);
                ListOfItemGroups.RemoveAt(ListOfItemGroups.CurrentIndex);
                ListOfItemGroups.Add(temp);
                MessageBox.Show($"Varegruppens navn er blevet opdateret til {ItemGroupName}");
                ItemGroupName = "";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }    
        }


        public void CreateItemGroup()
        {
            long itemGroupID;
            long parentItemGroupID;
            try
            {
                
                if (Regex.IsMatch(ItemGroupName, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
                {
                    if (ComboBoxOptions.CurrentIndex == -1)
                    {
                        itemGroupID = db.TableItemGroup.CreateItemGroup(ItemGroupName);
                        parentItemGroupID = 0;
                    }
                    else
                    {
                        parentItemGroupID = ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupID;
                        itemGroupID = db.TableItemGroup.CreateItemGroup(ItemGroupName,parentItemGroupID);
                    }
                    
                    var createdItem = new ItemGroup(ItemGroupName, parentItemGroupID, itemGroupID);
                    ListOfItemGroups.Add(createdItem);
                    MessageBox.Show($"{ItemGroupName} er blevet tilføjet til databasen");
                    ComboBoxIndex = -1;
                    ItemGroupName = "";
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
                ListOfItemGroups.Clear();
                if (SearchString == "")
                {
                    ListOfItemGroups.Populate();
                }
                else
                {
                    ListOfItemGroups.Populate(SearchString);
                }

                if (ListOfItemGroups.Count == 0)
                {
                    MessageBox.Show($"Kunne ikke finde nogen varegruppe med navnet {SearchString}");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");

            }

        }

        public void DeleteItemGroup()
        {
            try
            {
                ItemGroup selectedItem = ListOfItemGroups[ListOfItemGroups.CurrentIndex];
                db.TableItemGroup.DeleteItemGroup((long)selectedItem.ItemGroupID);
                ListOfItemGroups.RemoveAt(ListOfItemGroups.CurrentIndex);
                MessageBox.Show($"{selectedItem.ItemGroupName} blev slettet fra databasen");

            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }

        public void UpdateItemGroupName()
        {
            OldItemGroupName = ListOfItemGroups[ListOfItemGroups.CurrentIndex].ItemGroupName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
