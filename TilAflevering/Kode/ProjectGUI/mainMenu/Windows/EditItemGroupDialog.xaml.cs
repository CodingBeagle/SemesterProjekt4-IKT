using System.Windows;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for EditItemGroupDialog.xaml
    /// </summary>
    public partial class EditItemGroupDialog : Window
    {
        public ItemGroupViewModel ViewModel;
        public EditItemGroupDialog(ItemGroupViewModel view)
        {
            InitializeComponent();
            ViewModel = view;
            DataContext = ViewModel;
            ViewModel.UpdateItemGroupName();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
