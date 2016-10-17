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
            Microsoft.Win32.OpenFileDialog browse = new Microsoft.Win32.OpenFileDialog();
            browse.DefaultExt = ".png";

            Nullable<bool> result = browse.ShowDialog();


            if (result.HasValue && result.Value)
            {
                string filename = browse.FileName;
                filepathBox.Text = filename;
            }
        }
    }
}
