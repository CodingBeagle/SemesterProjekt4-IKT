using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.FloorplanLogic;
using MvvmFoundation.Wpf;

namespace mainMenu
{
    public class SectionShape
    {
        public string Name { get; set; }

        public  long ID { get; set; }

        public Shape Shape { get; set; }

        public double Top { get; set; }
        public double Left { get; set; }
    }

    public class StoreSectionViewModel: INotifyPropertyChanged
    { 
        public ICommand WindowLoadedCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand SelectCurrentStoreSectionCommand { get; private set; }
        public ICommand DeleteStoreSectionCommand { get; private set; }
        public ICommand EditStoreSectionCommand { get; private set; }
     
        public string NewSectionName { get; set; }

        public  string PreviousSectionName { get; set; }

        public ObservableCollection<SectionShape> ShapeCollection { get; set; }

        public long SelectedStoreSection = 0;

        private Window currentWindow { get; }

        private DatabaseService _db;
        private long _floorplanID = 1;
        private List<StoreSection> _storeSectionList;

        public StoreSectionViewModel(Window window)
        {
            try
            {
                _db = new DatabaseService(new SqlStoreDatabaseFactory());
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong" + e.Message);
            }
            ShapeCollection = new ObservableCollection<SectionShape>();

            WindowLoadedCommand = new RelayCommand(windowLoadedHandler);
            BackCommand = new RelayCommand(backHandler);
            SelectCurrentStoreSectionCommand = new RelayCommand<SectionShape>(selectCurrentStoreSectionHandler);
            DeleteStoreSectionCommand = new RelayCommand(deleteStoreSectionHandler, () => SelectedStoreSection != 0);
            EditStoreSectionCommand = new RelayCommand(editStoreSectionHandler, () => SelectedStoreSection != 0);
            currentWindow = window;
        }

        private void windowLoadedHandler()
        {
            _storeSectionList = _db.TableStoreSection.GetAllStoreSections(_floorplanID);
            foreach (var section in _storeSectionList)
            {
                SectionShape loadedSectionShape = new SectionShape();
                loadedSectionShape.Top = section.CoordinateY;
                loadedSectionShape.Left = section.CoordinateX;
                loadedSectionShape.Shape = ShapeButtonCreator.CreateShapeForButton();
                loadedSectionShape.Name = "Button" + section.StoreSectionID;
                loadedSectionShape.ID = section.StoreSectionID;

                ShapeCollection.Add(loadedSectionShape);
            }
        }
        private void backHandler()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            currentWindow.Close();
        }

        public void CreateStoreSection(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            SectionShape newSectionShape = new SectionShape();

            newSectionShape.Top = e.GetPosition(canvas).Y;
            newSectionShape.Left = e.GetPosition(canvas).X;

            newSectionShape.Shape = ShapeButtonCreator.CreateShapeForButton();
            
            ShapeCollection.Add(newSectionShape);

            AddSectionDialog newSectionDialog = new AddSectionDialog();
            newSectionDialog.ShowDialog();

            if (newSectionDialog.IsOKPressed)
            {
                long newStoreSectionID = _db.TableStoreSection.CreateStoreSection(newSectionDialog.SectionName, (long)newSectionShape.Left, (long)newSectionShape.Top, _floorplanID);
                newSectionShape.ID = newStoreSectionID;
                newSectionShape.Name = "Button" + newStoreSectionID;
            }
            else
            {
                ShapeCollection.Remove(newSectionShape);
            }
        }

        private void selectCurrentStoreSectionHandler(SectionShape shape)
        {
            SelectedStoreSection = shape.ID;
            Debug.WriteLine(SelectedStoreSection +" "+ shape.ID);
        }

   

        private void deleteStoreSectionHandler()
        {
            SectionShape shapeToDelete = null;
            string sectionShapeToDelete = "Button" + SelectedStoreSection;

            foreach (var shape in ShapeCollection)
            {
                if (shape.Name == sectionShapeToDelete)
                {
                    shapeToDelete = shape;
                }
            }

            if (shapeToDelete != null)
            {
                ShapeCollection.Remove(shapeToDelete);
            }

            _db.TableStoreSection.DeleteStoreSection(SelectedStoreSection);
        }

        private void editStoreSectionHandler()
        {
            StoreSection sectionToEdit = _db.TableStoreSection.GetStoreSection(SelectedStoreSection);

            NewSectionName = "";
            PreviousSectionName = sectionToEdit.Name;
            EditSectionDialog newSectionDialog = new EditSectionDialog(this, sectionToEdit.Name);
            newSectionDialog.ShowDialog();

            if (newSectionDialog.IsSectionNameChanged)
            {
                _db.TableStoreSection.UpdateStoreSectionName(SelectedStoreSection, NewSectionName);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
