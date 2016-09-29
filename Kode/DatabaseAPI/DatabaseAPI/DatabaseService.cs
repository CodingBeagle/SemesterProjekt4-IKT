using DatabaseAPI.Factories;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;

namespace DatabaseAPI
{
    public class DatabaseService
    {
        private ITableItem _tableItem;
        private ITableItemGroup _tableItemGroup;

        public DatabaseService(IDatabaseFactory databaseFactory)
        {
            _tableItem = databaseFactory.CreateTableItem();
            _tableItemGroup = databaseFactory.CreateTableItemGroup();
        }
    }
}

