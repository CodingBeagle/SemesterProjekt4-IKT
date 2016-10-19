using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using mainMenu.Models;

namespace mainMenu
{
    public static class ItemUtility
    {
        public static void SearchItem(DatabaseService db, string searchstring, DisplayItems listofItems )
        {
            try
            {
                var searchList = db.TableItem.SearchItems(searchstring);
                listofItems.Clear();
                listofItems.Populate(searchList);

                if (searchList.Count == 0)
                    MessageBox.Show($"Fandt ingen varer med navnet {searchstring}");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went horribly wrong: {exception.Message}");
            }
        }
    }
}
