using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public interface ITableItem
    {
        void CreateItem(string name, long itemGroup);
        void DeleteItem(long ID);
        List<Item> SearchItems(string itemName);
    }
}