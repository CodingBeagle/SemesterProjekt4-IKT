using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace mainMenu.RefreshLogic
{
    public class RefreshableImage
    {
        public BitmapImage Get(string filepath)
        {
            BitmapImage refreshableImage = new BitmapImage();
            refreshableImage.BeginInit();
            refreshableImage.UriSource = new Uri(filepath, UriKind.Relative);
            // .OnLoad makes sure WPF prevents keeping a lock on the file
            refreshableImage.CacheOption = BitmapCacheOption.OnLoad;
            // .IgnoreImageCache causes WPF to reread the image every time
            // Should be used when selected images needs to be refreshed
            refreshableImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            refreshableImage.EndInit();

            return refreshableImage;
        }
    }
}