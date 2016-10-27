using System.Windows;

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
    
            DataContext = new StoreSectionViewModel();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }
    }
}
