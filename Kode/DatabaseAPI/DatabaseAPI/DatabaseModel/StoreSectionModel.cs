using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.DatabaseModel
{
    public class StoreSectionModel
    {
        public long StoreSectionID { get; set; }
        public string Name { get; set; }
        public long CoordinateX { get; set; }
        public long CoordinateY { get; set; }
        public long FloorPlanID { get; set; }

        public StoreSectionModel(long storeSectionID, string name, long coordinateX, long coordinateY, long floorPlanID)
        {
            StoreSectionID = storeSectionID;
            Name = name;
            CoordinateX = coordinateX;
            CoordinateY = coordinateY;
            FloorPlanID = floorPlanID;
        }




    }
}
