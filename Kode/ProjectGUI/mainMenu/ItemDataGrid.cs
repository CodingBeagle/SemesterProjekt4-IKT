using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace mainMenu
{
    public class ItemDataGrid : DataGrid
    {
        public IList SelectedItemsList
        {
            get { return (IList) GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty,value);}
        }

        public static readonly DependencyProperty SelectedItemsListProperty = 
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(ItemDataGrid), new PropertyMetadata(null));
        public ItemDataGrid()
        {
            this.SelectionChanged += ItemDataGrid_SelectionChanged;
        }

        private void ItemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }

        
    }
}
