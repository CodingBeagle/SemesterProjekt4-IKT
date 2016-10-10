using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;

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

        public ITableFloorplan CreateTableFloorplan()
        {
            return new SqlTableFloorplan(new SqlConnectionStringFactory().CreateConnectionString());
        }
    }
}