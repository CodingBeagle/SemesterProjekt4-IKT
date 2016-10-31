using System.Windows;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AdminItemGroups.xaml
    /// </summary>
    public partial class AdminItemGroups : Window
    {
        public ItemGroupViewModel ViewModel = new ItemGroupViewModel();
        public AdminItemGroups()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            AddItemGroupDialog Thingy = new AddItemGroupDialog(ViewModel);
            Thingy.ShowDialog();
        }

        private void EditItems_Click(object sender, RoutedEventArgs e)
        {
            EditItemGroupDialog thingy = new EditItemGroupDialog(ViewModel);
            thingy.ShowDialog();
        }
    }
}
