using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace mainMenu.RefreshLogic
{
    public class ImageRefresh
    {
        public BitmapImage ImgRefresh(string filepath)
        {
            BitmapImage result = new BitmapImage();
            result.BeginInit();
            result.UriSource = new Uri(filepath, UriKind.Relative);
            // .OnLoad makes sure WPF prevents keeping a lock on the file
            result.CacheOption = BitmapCacheOption.OnLoad;
            // .IgnoreImageCache causes WPF to reread the image every time
            // Should be used when selected images needs to be refreshed
            result.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            result.EndInit();

            return result;
        }
    }
}