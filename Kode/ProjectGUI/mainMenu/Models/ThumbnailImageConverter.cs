using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using mainMenu.RefreshLogic;

namespace mainMenu.Models
{
    class ThumbnailImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string filePath = value.ToString();
                if (File.Exists(filePath))
                {
                    RefreshableImage refreshableImage = new RefreshableImage();
                    return refreshableImage.Get(filePath);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
