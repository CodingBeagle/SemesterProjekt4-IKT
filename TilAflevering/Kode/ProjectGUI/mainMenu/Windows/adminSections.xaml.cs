using System.Windows;
using DatabaseAPI;
using DatabaseAPI.Factories;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminSections.xaml
    /// </summary>
    public partial class adminSections : Window
    {
        private StoreSectionViewModel _viewModel;
        
        public adminSections()
        {
            InitializeComponent();
            _viewModel = new StoreSectionViewModel(new DatabaseService(new SqlStoreDatabaseFactory()), new ErrorMessageBox());
            DataContext = _viewModel;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }

        private void ItemsControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddSectionDialog newSectionDialog = new AddSectionDialog(_viewModel);
            newSectionDialog.ShowDialog();
        }


        private void editSectionBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSectionDialog newSectionDialog = new EditSectionDialog(_viewModel);
            newSectionDialog.ShowDialog();
        }
    }
}
