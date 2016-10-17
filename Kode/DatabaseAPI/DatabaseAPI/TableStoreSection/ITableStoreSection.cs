using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.Factories
{
    public interface ITableStoreSection
    {
        long CreateStoreSection(string storeSectionName, long coordinateX, long coordinateY, long floorPlanID);
        void DeleteStoreSection(long storeSectionID);
        void DeleteAllStoreSections(long floorPlanID);

        List<StoreSectionModel> GetAllStoreSections(long floorPlanID);
        StoreSectionModel GetStoreSection(long storeSectionID);

        void UpdateStoreSectionCoordinate(long storeSectionID, long coordinateX, long coordinateY);

        void UpdateStoreSectionName(long storeSectionID, string storeSectionName);
    }
}