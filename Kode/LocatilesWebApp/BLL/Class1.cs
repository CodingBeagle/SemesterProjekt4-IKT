using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using DatabaseAPI.TableItem;

namespace BLL
{
    public class Class1
    {
        public string getItem()
        {
            ITableItem ti = (new SqlStoreDatabaseFactory()).CreateTableItem();
            Item item = ti.GetItem(220);
            return item.Name;
        }
    }
}
