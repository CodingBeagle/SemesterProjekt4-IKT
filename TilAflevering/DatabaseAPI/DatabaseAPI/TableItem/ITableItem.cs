using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public interface ITableItem
    {
        long CreateItem(string itemName, long itemGroup);
        void DeleteItem(long itemID);
        List<Item> SearchItems(string itemName);
        Item GetItem(long itemID);
    }
}