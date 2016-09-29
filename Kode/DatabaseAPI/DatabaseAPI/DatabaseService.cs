using DatabaseAPI.Factories;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;

namespace DatabaseAPI
{
    public class DatabaseService
    {
        public ITableItem TableItem { get; private set; }

        public ITableItemGroup TableItemGroup { get; private set; }

        public DatabaseService(IDatabaseFactory databaseFactory)
        {
            TableItem = databaseFactory.CreateTableItem();
            TableItemGroup = databaseFactory.CreateTableItemGroup();
        }
    }
}

