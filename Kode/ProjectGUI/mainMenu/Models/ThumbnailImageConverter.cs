using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

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
                    BitmapImage result = new BitmapImage();
                    result.BeginInit();
                    result.UriSource = new Uri(filePath, UriKind.Relative);
                    // .OnLoad makes sure WPF prevents keeping a lock on the file
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    // .IgnoreImageCache causes WPF to reread the image every time
                    // Should be used when selected images needs to be refreshed
                    result.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    result.EndInit();
                    return result;
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
