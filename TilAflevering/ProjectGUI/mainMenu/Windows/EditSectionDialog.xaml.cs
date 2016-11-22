using System.Text.RegularExpressions;
using System.Windows;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for EditSectionDialog.xaml
    /// </summary>
    public partial class EditSectionDialog : Window
    {

        private StoreSectionViewModel _viewModel;
        public EditSectionDialog(StoreSectionViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
            viewModel.PrepareSectionToEdit();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
