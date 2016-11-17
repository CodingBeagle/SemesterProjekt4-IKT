using System.Collections.Generic;

namespace LocatilesWebApp.Models
{
    public interface IBLL
    {
        void GetFloorPlan(string filepath);
        List<PresentationItemGroup> GetPresentationItemGroups(string searchString);
    }
}