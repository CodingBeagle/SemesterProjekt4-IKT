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
    /// Interaction logic for adminSections.xaml
    /// </summary>
    public partial class adminSections : Window
    {
        public adminSections()
        {
            InitializeComponent();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();

        }
    }
}
