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
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.Models;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AddItemDialog.xaml
    /// </summary>
    public partial class AddItemDialog : Window
    {
        private AdminItemViewModel viewModel;
        public AddItemDialog(AdminItemViewModel view)
        {
            InitializeComponent();
            viewModel = view;
            try
            {
                this.DataContext = viewModel;
                ItemGroupComboBox.ItemsSource = viewModel.ItemGroupComboBoxList;
                ItemGroupComboBox.DisplayMemberPath = "ItemGroupName";
                
            }
            catch (Exception e)
            {
                MessageBox.Show($"Der er noget der gik galt: {e.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
