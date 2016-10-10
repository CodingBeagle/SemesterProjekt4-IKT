using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;

namespace DatabaseAPI.Factories
{
    public interface IStoreDatabaseFactory
    {
        ITableItem CreateTableItem();
        ITableItemGroup CreateTableItemGroup();
        ITableStoreSection CreateTableStoreSection();
        ITableFloorplan CreateTableFloorplan();
    }
}
