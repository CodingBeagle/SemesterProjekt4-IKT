using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public interface ITableItem
    {
        void CreateItem(string name, int itemGroup);
        void DeleteItem(int ID);
        List<Item> SearchItems(string itemName);
    }
}