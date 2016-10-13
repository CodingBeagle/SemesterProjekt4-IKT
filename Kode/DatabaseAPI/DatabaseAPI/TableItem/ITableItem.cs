using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public interface ITableItem
    {
        long CreateItem(string name, long itemGroup);
        void DeleteItem(long ID);
        List<Item> SearchItems(string itemName);
        Item GetItem(long itemID);
    }
}