using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.FloorplanLogic;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminSections.xaml
    /// </summary>
    public partial class adminSections : Window
    {
        private DatabaseService _db;
        
        
 
        private long _currentlySelectedStoreSectionID = 0;

        public adminSections()
        {
            InitializeComponent();
    
            DataContext = new StoreSectionViewModel(this);
        }



   
    }
}
