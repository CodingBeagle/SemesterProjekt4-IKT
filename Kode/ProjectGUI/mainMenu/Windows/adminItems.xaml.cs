using System.Windows;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminItems.xaml
    /// </summary>
    public partial class adminItems : Window
    {
        public ItemViewModel viewModel = new ItemViewModel();

        public adminItems()
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void BackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            AddItemDialog newAddItemDialog = new AddItemDialog(viewModel);
            newAddItemDialog.ShowDialog();
        }
    }
}
