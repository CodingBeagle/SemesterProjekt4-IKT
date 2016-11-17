using System.Collections.Generic;

namespace LocatilesWebApp.Models
{
    public interface IBLL
    {
        void GetFloorPlan(string filepath);
        List<string> SearchOptimization(string searchString);
        List<PresentationItemGroup> GetPresentationItemGroups(List<string> searchStringList);
    }
}