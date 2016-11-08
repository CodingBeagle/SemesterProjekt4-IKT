using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using mainMenu.Models;
using mainMenu.ViewModels;

namespace mainMenu
{
    public static class ItemUtility
    {
        public static void SearchItem(IDatabaseService db, string searchstring, DisplayItems listofItems, IMessageBox messageBox )
        {
            try
            {
                var searchList = db.TableItem.SearchItems(searchstring);
                listofItems.Clear();
                listofItems.Populate(searchList);

                if (searchList.Count == 0)
                    messageBox.OpenMessageBox($"Fandt ingen varer med navnet {searchstring}");
            }
            catch (Exception exception)
            {
                messageBox.OpenMessageBox("Noget gik galt! Check Debug for fejlmeddelelse");
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
