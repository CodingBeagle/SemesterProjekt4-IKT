using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PresentationItem
    {
        public string Itemname { get; set; }
        public string Itemgroupname { get; set; }
        public List<Point> ItemPlacementList { get; set; }

        public  PresentationItem(string itemname, string itemgroupname, List<Point> coordinates)
        {
            Itemname = itemname;
            Itemgroupname = itemgroupname;
            ItemPlacementList = coordinates;
        }

    }

    public class PresentationItemGroup
    {
        public  string Name { get; set; }
        public List<PresentationItem> PresentationItems { get; set; }

        public PresentationItemGroup(string name, List<PresentationItem> presentationItems )
        {
            Name = name;
            PresentationItems = presentationItems;
        }
    }
}
