    using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using mainMenu.ViewModels;
using Microsoft.Win32;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AdminFloorplan.xaml
    /// </summary>
    public partial class AdminFloorplan : Window
    {
        readonly FloorplanViewModel viewModel = new FloorplanViewModel();

        public AdminFloorplan()
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
