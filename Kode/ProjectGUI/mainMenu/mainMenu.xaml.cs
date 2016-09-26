using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        
        //Logo tilføjes som billede
        private void Logo_OnLoaded(object sender, RoutedEventArgs e)
        {
            BitmapImage Logo = new BitmapImage();
            Logo.BeginInit();
            Logo.UriSource = new Uri("C:\\Locatiles.png");
            Logo.EndInit();

            var logoImage = sender as Image;
            Debug.Assert(logoImage != null, "logoImage != null");
            logoImage.Source = Logo;
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
    }
}
