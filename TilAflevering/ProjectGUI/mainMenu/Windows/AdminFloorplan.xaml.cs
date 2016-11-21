using System.Windows;
using DatabaseAPI;
using DatabaseAPI.Factories;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AdminFloorplan.xaml
    /// </summary>
    public partial class AdminFloorplan : Window
    {
        readonly FloorplanViewModel viewModel = new FloorplanViewModel(new DatabaseService(new SqlStoreDatabaseFactory()), new ImageFileBrowser());

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
