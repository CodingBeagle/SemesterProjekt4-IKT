using Microsoft.Win32;

namespace mainMenu.ViewModels
{
    public class ImageFileBrowser : IBrowseFileService
    {
        public string FileName { get; private set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog floorplanBrowser = new OpenFileDialog();

            floorplanBrowser.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            bool? browseResult = floorplanBrowser.ShowDialog();

            if (!browseResult.HasValue || !browseResult.Value)
                return false;

            FileName = floorplanBrowser.FileName;
            return true;
        }
    }
}
