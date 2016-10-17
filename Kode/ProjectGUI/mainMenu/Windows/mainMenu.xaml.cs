using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Resources;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            adminItems adminItemsWindow = new adminItems();
            adminItemsWindow.Show();
            Close();

        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addSectionBtn_Click(object sender, RoutedEventArgs e)
        {
            adminSections adminSectionssWindow = new adminSections();
            adminSectionssWindow.Show();
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AdminFloorplan adminFloorplan = new AdminFloorplan();
            adminFloorplan.Show();
            Close();
        }
    }
}
