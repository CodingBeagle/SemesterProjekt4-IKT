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
using Microsoft.Win32;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AdminFloorplan.xaml
    /// </summary>
    public partial class AdminFloorplan : Window
    {
        public AdminFloorplan()
        {
            InitializeComponent();
        }

        private void LoadFloorplanImage()
        {

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();

        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            var browse = new OpenFileDialog();

            browse.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";


            var result = browse.ShowDialog();


            if (!result.HasValue || !result.Value) return;
            var filename = browse.FileName;
            filepathBox.Text = filename;
        }
    }
}
