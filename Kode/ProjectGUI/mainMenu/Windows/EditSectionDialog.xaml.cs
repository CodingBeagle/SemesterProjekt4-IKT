using System.Text.RegularExpressions;
using System.Windows;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for EditSectionDialog.xaml
    /// </summary>
    public partial class EditSectionDialog : Window
    {

        public bool IsSectionNameChanged { get; private set; }

        private StoreSectionViewModel _viewModel;
        public EditSectionDialog(StoreSectionViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(NewSectionNameTextBox.Text, @"^[a-zA-Z0-9-øØ-æÆ-åÅ\s]+$"))
            {
                IsSectionNameChanged = true;
                
                Close();
            }
            else
            {
                MessageBox.Show("Navnet på en sektion må kun indeholde bogstaver og tal");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsSectionNameChanged = false;
            Close();
        }
    }
}
