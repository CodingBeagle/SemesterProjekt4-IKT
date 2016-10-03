using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;

namespace DatabaseAPI.Factories
{
    public class SqlDatabaseFactory : IDatabaseFactory
    {
        public ITableItem CreateTableItem()
        {
            return new SqlTableItem(new SqlConnectionStringFactory().CreateConnectionString());
        }

        public ITableItemGroup CreateTableItemGroup()
        {
            return new SqlTableItemGroup(new SqlConnectionStringFactory().CreateConnectionString());
        }
    }
}