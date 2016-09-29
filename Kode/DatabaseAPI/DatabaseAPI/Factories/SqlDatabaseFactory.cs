using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;

namespace DatabaseAPI.Factories
{
    public class SqlDatabaseFactory : IDatabaseFactory
    {
        public ITableItem CreateTableItem()
        {
            throw new System.NotImplementedException();
        }

        public ITableItemGroup CreateTableItemGroup()
        {
            throw new System.NotImplementedException();
        }
    }
}