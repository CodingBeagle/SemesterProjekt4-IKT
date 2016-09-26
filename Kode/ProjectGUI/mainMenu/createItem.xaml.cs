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
    /// Interaction logic for createItem.xaml
    /// </summary>
    public partial class createItem : Window
    {
        public createItem()
        {
            InitializeComponent();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            adminItems adminitems = new adminItems();
            adminitems.Show();
            Close();
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button Event added. Implement functionality later");
        }
    }
}
