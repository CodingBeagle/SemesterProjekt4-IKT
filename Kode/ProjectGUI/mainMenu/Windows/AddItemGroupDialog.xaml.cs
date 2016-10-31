using System.Windows;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AdminItemGroupDialog.xaml
    /// </summary>
    public partial class AddItemGroupDialog : Window
    {
        public ItemGroupViewModel ViewModel { get; set; }
        public AddItemGroupDialog(ItemGroupViewModel view)
        {
            InitializeComponent();
            ViewModel = view;
            ViewModel.ComboBoxIndex = -1;
            DataContext = ViewModel;
            ItemGroupComboBox.ItemsSource = ViewModel.ListOfItemGroups;
            ItemGroupComboBox.DisplayMemberPath = "ItemGroupName";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
