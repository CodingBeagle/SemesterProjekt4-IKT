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
        public static Shape CreateShapeForButton()
        {
            Shape newSectionShape = new Ellipse();
            newSectionShape.Width = 10;
            newSectionShape.Height = 10;
            newSectionShape.Fill = new SolidColorBrush(Colors.Red);

            return newSectionShape;
        }
    }
}
