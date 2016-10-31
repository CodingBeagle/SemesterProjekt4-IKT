namespace mainMenu.ViewModels
{
    public interface IBrowseFileService
    {
        string FileName { get; }
        bool OpenFileDialog();
    }
}
