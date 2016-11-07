namespace DatabaseAPI.DatabaseModel
{
    public class Floorplan
    {
        public long FloorplanID { get; set; }
        public string Name { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }

        public Floorplan(long floorplanID, string name, long width, long height)
        {
            FloorplanID = floorplanID;
            Name = name;
            Width = width;
            Height = height;
        }
    }
}
