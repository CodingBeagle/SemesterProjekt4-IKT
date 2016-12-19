using System.Windows;

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

        private void addSectionBtn_Click(object sender, RoutedEventArgs e)
        {
            adminSections adminSectionssWindow = new adminSections();
            adminSectionssWindow.Show();
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AdminFloorplan adminFloorplan = new AdminFloorplan();
            adminFloorplan.Show();
            Close();
        }

        private void addItemGrpBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminItemGroups adminItemGroup = new AdminItemGroups();
            adminItemGroup.Show();
            Close();
        }
    }
}
