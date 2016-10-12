﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminItems.xaml
    /// </summary>
    public partial class adminItems : Window
    {
        private List<Item> searchList;
        public DisplayItems displayItemses = new DisplayItems();
        private DatabaseService db;

        public adminItems()
        {
            InitializeComponent();
            //SearchResultGrid.ItemsSource = searchList;
            SearchResultGrid.DataContext = displayItemses;
            DeleteItems.DataContext = displayItemses;

            try
            {
                db = new DatabaseService(new SqlStoreDatabaseFactory());

            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went horribly wrong: {e.Message}");
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();


        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            AddItemDialog newAddItemDialog = new AddItemDialog(displayItemses);
            newAddItemDialog.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                searchList = db.TableItem.SearchItems(SearchBox.Text);
                displayItemses.Clear();
                //SearchResultGrid.ItemsSource = searchList;
                foreach (var item in searchList)
                {
                    DisplayItem displayItem = new DisplayItem(item);
                    displayItemses.Add(displayItem);
                }
                SearchResultGrid.ItemsSource = null;
                SearchResultGrid.ItemsSource = displayItemses;

                if (searchList.Count == 0)
                    MessageBox.Show($"Fandt ingen varer med navnet {SearchBox.Text}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }
    }
}
