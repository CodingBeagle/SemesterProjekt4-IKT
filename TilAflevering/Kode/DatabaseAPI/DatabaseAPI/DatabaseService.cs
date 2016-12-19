using DatabaseAPI.Factories;
using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;
using DatabaseAPI.TableItemSectionPlacement;

namespace DatabaseAPI
{
    public class DatabaseService : IDatabaseService
    {
        public ITableItem TableItem { get; private set; }
        public ITableItemGroup TableItemGroup { get; private set; }
        public ITableFloorplan TableFloorplan { get; private set; }
        public ITableStoreSection TableStoreSection { get; private set; }
        public ITableItemSectionPlacement TableItemSectionPlacement { get; private set; }

        public DatabaseService(IStoreDatabaseFactory storeDatabaseFactory)
        {
            TableItem = storeDatabaseFactory.CreateTableItem();
            TableItemGroup = storeDatabaseFactory.CreateTableItemGroup();
            TableFloorplan = storeDatabaseFactory.CreateTableFloorplan();
            TableStoreSection = storeDatabaseFactory.CreateTableStoreSection();
            TableItemSectionPlacement = storeDatabaseFactory.CreateTableItemSectionPlacement();
        }
    }
}

