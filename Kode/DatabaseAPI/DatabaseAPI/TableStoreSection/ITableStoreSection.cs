﻿using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.Factories
{
    public interface ITableStoreSection
    {
        void CreateStoreSection(string storeSectionName, long coordinateX, long coordinateY, long floorPlanID);
        void DeleteStoreSection(long storeSectionID);
        void DeleteAllStoreSections(long floorPlanID);

        List<StoreSection> GetAllStoreSections(long floorPlanID);
        StoreSection GetStoreSection(long storeSectionID);

        void UpdateStoreSection(long storeSectionID, string storeSectionName, long coordinateX, long coordinateY,
            long floorPlanID);
    }
}