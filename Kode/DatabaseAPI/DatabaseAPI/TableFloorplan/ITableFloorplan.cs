using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableFloorplan
{
    public interface ITableFloorplan
    {
        void UploadFloorplan(string name, int width, int height, string filePath);
    }
}
