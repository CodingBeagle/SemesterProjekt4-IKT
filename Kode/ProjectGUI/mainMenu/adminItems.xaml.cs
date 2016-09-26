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

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminItems.xaml
    /// </summary>
    public partial class adminItems : Window
    {
        public adminItems()
        {
            InitializeComponent();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
            
            
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var createItem = new createItem();
            createItem.Show();
            Close();

        }

        private void editItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var editItem = new editItem();
            editItem.Show();
            Close();
        }

        private void deleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            var deleteItem = new deleteItem();
            deleteItem.Show();
            Close();
        }
    }
}
