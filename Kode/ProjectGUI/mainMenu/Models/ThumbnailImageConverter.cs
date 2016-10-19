using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using mainMenu.RefreshLogic;

namespace mainMenu.Models
{
    /// <summary>
    /// The ThumbnailImageConverter class has the responsibility of returning a BitmapImage
    /// To the view which does not hold a lock on the referenced image on the filesystem.
    /// This makes it so that images in the view can be refreshed during runtime of the
    /// Application
    /// </summary>
    public class ThumbnailImageConverter : IValueConverter
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

        // We do not need the ConvertBack method for this usage, therefore it has not been implemented
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
