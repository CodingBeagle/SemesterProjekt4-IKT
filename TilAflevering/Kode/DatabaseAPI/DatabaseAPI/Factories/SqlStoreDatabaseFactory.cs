using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;
using DatabaseAPI.TableItemSectionPlacement;
using DatabaseAPI.TableStoreSection;

namespace DatabaseAPI.Factories
{
    public class SqlStoreDatabaseFactory : IStoreDatabaseFactory
    {
        public ITableItem CreateTableItem()
        {
            return new SqlTableItem(new SqlConnectionStringFactory().CreateConnectionString());
        }

        public ITableItemGroup CreateTableItemGroup()
        {
            return new SqlTableItemGroup(new SqlConnectionStringFactory().CreateConnectionString());
        }

        public ITableStoreSection CreateTableStoreSection()
        {
            return new SqlTableStoreSection(new SqlConnectionStringFactory().CreateConnectionString());
        }

        public ITableFloorplan CreateTableFloorplan()
        {
            return new SqlTableFloorplan(new SqlConnectionStringFactory().CreateConnectionString());
        }

        public ITableItemSectionPlacement CreateTableItemSectionPlacement()
        {
            return new SqlTableItemSectionPlacement(new SqlConnectionStringFactory().CreateConnectionString());
        }
    }
}
