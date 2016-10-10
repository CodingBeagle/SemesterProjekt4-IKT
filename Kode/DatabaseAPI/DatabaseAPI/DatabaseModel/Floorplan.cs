using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAPI.DatabaseModel
{
    public class Floorplan
    {
        public byte[] ImageData { get; set; }

        public Floorplan(byte[] imageData)
        {
            ImageData = imageData;
        }
    }
}
