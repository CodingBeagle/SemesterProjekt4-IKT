using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItemSectionPlacement
{
    public interface ITableItemSectionPlacement
    {
        long PlaceItem(long itemID, long sectionID);

        List<Item> ListItemsInSection(long sectionID);

        StoreSection FindPlacementByItem(long ItemID);

        void DeletePlacement(long itemSectionPlacementID);
    }
}