﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for EditSectionDialog.xaml
    /// </summary>
    public partial class EditSectionDialog : Window
    {
        public string newSectionName { get; private set; }
        public  string previousSectionName { get; set; }

        public bool IsSectionNameChanged { get; private set; }
        public EditSectionDialog(string sectionNameToEdit)
        {
            InitializeComponent();

            DataContext = this;

            previousSectionName = sectionNameToEdit;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(NewSectionNameTextBox.Text, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
            {
                IsSectionNameChanged = true;
                newSectionName = NewSectionNameTextBox.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Navnet på en sektion må kun indeholde bogstaver og tal");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsSectionNameChanged = false;
            Close();
        }
    }
}
