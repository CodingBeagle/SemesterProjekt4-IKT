using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItemSectionPlacement
{
    public interface ITableItemSectionPlacement
    {
        void PlaceItem(long itemID, long sectionID);

        List<Item> ListItemsInSection(long sectionID);

        List<StoreSection> FindPlacementsByItem(long ItemID);

        void DeleteAllPlacementsInSection(long sectionId);

        void DeletePlacementByItem(long itemId);
    }
}