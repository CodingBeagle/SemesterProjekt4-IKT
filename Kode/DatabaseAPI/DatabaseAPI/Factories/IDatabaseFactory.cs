using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;

namespace DatabaseAPI.Factories
{
    public interface IDatabaseFactory
    {
        ITableItem CreateTableItem();
        ITableItemGroup CreateTableItemGroup();
    }
}