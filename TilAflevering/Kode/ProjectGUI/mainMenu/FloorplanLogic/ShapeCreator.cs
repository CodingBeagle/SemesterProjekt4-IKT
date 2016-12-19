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
    public static class ShapeCreator
    {
        public static Shape CreateShape(Point point)
        {
            Shape newSectionShape = new Ellipse();
            newSectionShape.Width = 10;
            newSectionShape.Height = 10;
            newSectionShape.Fill = new SolidColorBrush(Colors.Red);

            Canvas.SetTop(newSectionShape, point.Y);
            Canvas.SetLeft(newSectionShape, point.X);

            return newSectionShape;
        }
    }
}
