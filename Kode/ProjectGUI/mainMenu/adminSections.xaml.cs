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
        private List<StoreSection> _storeSectionList;
        // To be deleted when we have method to get floorplan ID
        private long _floorplanID = 1;
        private Dictionary<string, long> storeSectionMapping = new Dictionary<string, long>();
        private long _currentlySelectedStoreSectionID = 0;

        public adminSections()
        {
            InitializeComponent();
            try
            {
                _db = new DatabaseService(new SqlStoreDatabaseFactory());
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong" + e.Message);
            }
        }

        private void sectionButton_Click(object sender, RoutedEventArgs e)
        {
            Button source = (Button) e.Source;

            storeSectionMapping.TryGetValue(source.Name, out _currentlySelectedStoreSectionID);
            //deleteSectionBtn.IsEnabled = true;

            Debug.WriteLine("Button name that was clicked: " + source.Name + " " + _currentlySelectedStoreSectionID);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentlySelectedStoreSectionID == 0)
            {
                deleteSectionBtn.IsEnabled = false;
            }
          

            _storeSectionList = _db.TableStoreSection.GetAllStoreSections(_floorplanID);
            foreach (var section in _storeSectionList)
            {
                Button loadedShapeButton = ShapeButtonCreator.CreateShapeButton(section.CoordinateX, section.CoordinateY);

                AddNewButtonToDictionary(section.StoreSectionID, loadedShapeButton);

                canvas.Children.Add(loadedShapeButton);
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
            Button newSectionShapeButton = ShapeButtonCreator.CreateShapeButton(retrievedPoint.X,retrievedPoint.Y);
            canvas.Children.Add(newSectionShapeButton);

            AddSectionDialog newSectionDialog = new AddSectionDialog();
            newSectionDialog.ShowDialog();

            if (newSectionDialog.IsOKPressed)
            {
                long newStoreSectionID = _db.TableStoreSection.CreateStoreSection(newSectionDialog.SectionName,(long)retrievedPoint.X,(long)retrievedPoint.Y, _floorplanID);
                AddNewButtonToDictionary(newStoreSectionID, newSectionShapeButton);
            }
            else
            {
                canvas.Children.Remove(newSectionShapeButton);
            }
        }

        private void AddNewButtonToDictionary(long sectionID, Button associatedButton)
        {
            string newButtonName = "Button" + sectionID;

            associatedButton.Name = newButtonName;
            storeSectionMapping.Add(newButtonName, sectionID);

            associatedButton.Click += sectionButton_Click;
        }

        private void deleteSectionBtn_Click(object sender, RoutedEventArgs e)
        {
            _db.TableStoreSection.DeleteStoreSection(_currentlySelectedStoreSectionID);
        }
    }
}
