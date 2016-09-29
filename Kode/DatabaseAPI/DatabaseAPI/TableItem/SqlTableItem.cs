using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public class SqlTableItem : ITableItem
    {
        public void CreateItem(string name, int itemGroup)
        {
            string SQLInsertCommand = $"INSERT INTO Item";
        }

        public void DeleteItem(int ID)
        {
            throw new System.NotImplementedException();
        }

        public List<Item> SearchItems(string itemName)
        {
            throw new System.NotImplementedException();
        }
    }
}