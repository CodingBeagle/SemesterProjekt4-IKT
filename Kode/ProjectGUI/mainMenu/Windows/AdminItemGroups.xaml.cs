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
    /// Interaction logic for AdminItemGroups.xaml
    /// </summary>
    public partial class AdminItemGroups : Window
    {
        public ItemGroupViewModel ViewModel = new ItemGroupViewModel();
        public AdminItemGroups()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            AddItemGroupDialog Thingy = new AddItemGroupDialog(ViewModel);
            Thingy.ShowDialog();
        }

        private void EditItems_Click(object sender, RoutedEventArgs e)
        {
            EditItemGroupDialog thingy = new EditItemGroupDialog(ViewModel);
            thingy.ShowDialog();
        }
    }
}
