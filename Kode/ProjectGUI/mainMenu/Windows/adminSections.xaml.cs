using System.Windows;
using DatabaseAPI;
using DatabaseAPI.Factories;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminSections.xaml
    /// </summary>
    public partial class adminSections : Window
    {
        private StoreSectionViewModel viewModel;
        public adminSections()
        {
            InitializeComponent();
            viewModel = new StoreSectionViewModel(new DatabaseService(new SqlStoreDatabaseFactory()));
            DataContext = viewModel;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }

        private void ItemsControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddSectionDialog newSectionDialog = new AddSectionDialog(viewModel);
            newSectionDialog.ShowDialog();
        }


        private void editSectionBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSectionDialog newSectionDialog = new EditSectionDialog(viewModel);
            newSectionDialog.ShowDialog();
        }
    }
}
