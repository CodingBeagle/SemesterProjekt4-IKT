﻿using System.Text.RegularExpressions;
using System.Windows;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AddSectionDialog.xaml
    /// </summary>
    public partial class AddSectionDialog : Window
    {
        public bool IsOKPressed { get; private set; }

        public AddSectionDialog(StoreSectionViewModel viewModel)
        {
            InitializeComponent();
            IsOKPressed = false;
            DataContext = viewModel;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateSectionButton_OnClick(object sender, RoutedEventArgs e)
        {
           Close();
        }
    }
}
