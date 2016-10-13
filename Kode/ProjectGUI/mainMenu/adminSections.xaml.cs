using System;
using System.Collections.Generic;
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
using DatabaseAPI.Factories;
using mainMenu.FloorplanLogic;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminSections.xaml
    /// </summary>
    public partial class adminSections : Window
    {
        private DatabaseService db;
        private bool _isOKPressed = false;
        private PointCollection SectionPoints = new PointCollection();
        public adminSections()
        {
            InitializeComponent();
            try
            {
                db = new DatabaseService(new SqlStoreDatabaseFactory());
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong" + e.Message);
            }
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Close();

        }


        private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point retrievedPoint = e.GetPosition(canvas);
            Shape newSectionShape = ShapeCreator.CreateShape(retrievedPoint);
            canvas.Children.Add(newSectionShape);

            AddSectionDialog newSectionDialog = new AddSectionDialog();
            newSectionDialog.ShowDialog();

            if (newSectionDialog.IsOKPressed)
            {
                
                SectionPoints.Add(retrievedPoint);
                // Create section on database
              
            }
            else
            {
                canvas.Children.Remove(newSectionShape);
            }
            
            

        }
    }
}
