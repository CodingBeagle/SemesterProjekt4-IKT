using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;
using DatabaseAPI.TableItemSectionPlacement;

namespace DatabaseAPI.Factories
{
    public interface IStoreDatabaseFactory
    {
        ITableItem CreateTableItem();
        ITableItemGroup CreateTableItemGroup();
        ITableStoreSection CreateTableStoreSection();
        ITableFloorplan CreateTableFloorplan();
        ITableItemSectionPlacement CreateTableItemSectionPlacement();

    }
}
