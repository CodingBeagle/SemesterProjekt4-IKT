using System;
using System.Diagnostics;
using DatabaseAPI;
using mainMenu.ViewModels;

namespace mainMenu.Models
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
