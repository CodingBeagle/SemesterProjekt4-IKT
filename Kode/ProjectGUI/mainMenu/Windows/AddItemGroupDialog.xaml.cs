using System;
using System.Collections.Generic;
using System.Linq;
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
using mainMenu.Models;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AdminItemGroupDialog.xaml
    /// </summary>
    public partial class AddItemGroupDialog : Window
    {
        public ItemGroupViewModel ViewModel { get; set; }
        public AddItemGroupDialog(ItemGroupViewModel view)
        {
            InitializeComponent();
            ViewModel = view;
            ViewModel.ComboBoxIndex = -1;
            DataContext = ViewModel;
            ItemGroupComboBox.ItemsSource = ViewModel.ListOfItemGroups;
            ItemGroupComboBox.DisplayMemberPath = "ItemGroupName";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
