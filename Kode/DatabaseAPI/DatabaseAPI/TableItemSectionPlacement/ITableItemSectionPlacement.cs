using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItemSectionPlacement
{
    public interface ITableItemSectionPlacement
    {
        void PlaceItem(long itemID, long sectionID);

        List<Item> ListItemsInSection(long sectionID);

        StoreSectionModel FindPlacementByItem(long ItemID);

        void DeleteAllPlacementsInSection(long sectionId);

        void DeletePlacementByItem(long itemId);
    }
}