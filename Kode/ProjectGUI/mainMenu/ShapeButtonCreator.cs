using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace mainMenu.FloorplanLogic
{
    public static class ShapeButtonCreator
    {
        public static Button CreateShapeButton(double pointX, double pointY)
        {
            Shape newSectionShape = new Ellipse();
            newSectionShape.Width = 10;
            newSectionShape.Height = 10;
            newSectionShape.Fill = new SolidColorBrush(Colors.Red);

            Button shapeButton = new Button();
            shapeButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
            shapeButton.Background = new SolidColorBrush(Colors.Transparent);
            shapeButton.Content = newSectionShape;

            Canvas.SetTop(shapeButton, pointY);
            Canvas.SetLeft(shapeButton, pointX);

            return shapeButton;
        }
    }
}
