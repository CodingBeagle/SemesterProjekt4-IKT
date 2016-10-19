namespace DatabaseAPI.TableFloorplan
{
    public interface ITableFloorplan
    {
        void UploadFloorplan(string name, int width, int height, string filePath);
        void DownloadFloorplan();
    }
}
