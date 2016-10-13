using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.DatabaseModel
{
    public class Floorplan
    {
        public long FloorplanID { get; private set; }

        public string FloorplanName { get; private set; }

        public Floorplan(long ID, string floorplanName)
        {
            FloorplanName = floorplanName;
        }
    }
}
