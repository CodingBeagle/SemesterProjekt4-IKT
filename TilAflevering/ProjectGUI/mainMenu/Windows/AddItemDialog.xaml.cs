using System;
using System.Windows;
using mainMenu.ViewModels;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for AddItemDialog.xaml
    /// </summary>
    public partial class AddItemDialog : Window
    {
        private ItemViewModel viewModel;
        public AddItemDialog(ItemViewModel view)
        {
            InitializeComponent();
            viewModel = view;
            try
            {
                this.DataContext = viewModel;
                ItemGroupComboBox.ItemsSource = viewModel.ItemGroupComboBoxList;
                ItemGroupComboBox.DisplayMemberPath = "ItemGroupName";
                
            }
            catch (Exception e)
            {
                MessageBox.Show($"Der er noget der gik galt: {e.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
