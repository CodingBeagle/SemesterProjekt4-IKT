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
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for EditItemGroupDialog.xaml
    /// </summary>
    public partial class EditItemGroupDialog : Window
    {
        public AdminItemGroupsViewModel ViewModel;
        public EditItemGroupDialog(AdminItemGroupsViewModel view)
        {
            InitializeComponent();
            ViewModel = view;
            DataContext = ViewModel;
            ViewModel.UpdateItemGroupName();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
