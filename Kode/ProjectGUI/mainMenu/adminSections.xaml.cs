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
using mainMenu.FloorplanLogic;

namespace mainMenu
{
    /// <summary>
    /// Interaction logic for adminSections.xaml
    /// </summary>
    public partial class adminSections : Window
    {
        private PointCollection SectionPoints = new PointCollection();
        public adminSections()
        {
            InitializeComponent();
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

            // New dialog with bool to check if okay was pressed or not
            SectionPoints.Add(retrievedPoint);

        }
    }
}
