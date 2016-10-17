using System;
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
    /// Interaction logic for AddSectionDialog.xaml
    /// </summary>
    public partial class AddSectionDialog : Window
    {
        public bool IsOKPressed { get; private set; }
        public string SectionName { get; private set; } 
        public AddSectionDialog()
        {
            InitializeComponent();
            IsOKPressed = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (Regex.IsMatch(SectionNameTextBox.Text, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
            {
                IsOKPressed = true;
                SectionName = SectionNameTextBox.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Navnet på en sektion må kun indeholde bogstaver og tal");
            }
           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
