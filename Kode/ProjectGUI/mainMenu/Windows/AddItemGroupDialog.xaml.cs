﻿using System.Windows;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AdminItemGroupDialog.xaml
    /// </summary>
    public partial class AddItemGroupDialog : Window
    {

        public AddItemGroupDialog(ItemGroupViewModel view)
        {
            InitializeComponent();
            DataContext = view;
            view.ComboBoxIndex = -1;
            ItemGroupComboBox.DisplayMemberPath = "ItemGroupName";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
