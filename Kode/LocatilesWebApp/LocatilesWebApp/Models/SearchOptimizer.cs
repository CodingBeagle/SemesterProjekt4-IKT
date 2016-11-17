using System.Collections.Generic;
using System.Linq;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace LocatilesWebApp.Models
{
    public class SearchOptimizer : ISearchOptimizer
    {

        private DatabaseService _db = new DatabaseService(new SqlStoreDatabaseFactory());
        public List<Item> SearchOptimization(string searchString)
        {
            List<string> _searchList = new List<string>();
            List<Item> _searchresultItems = new List<Item>();
            if (searchString.ToLower().Contains(' '))
            {
                string[] sa = searchString.Split(' ');
                sa = sa.Where(str => str != "").ToArray();
                for (int s = 0; s < sa.Length; s++)
                {
                    _searchList.Add(sa[s]);
                }
            }
            else _searchList.Add(searchString);

            foreach (var s in _searchList)
            {
                List<Item> _itemSearchWordResults = _db.TableItem.SearchItems(s);
                _searchresultItems.AddRange(_itemSearchWordResults.FindAll(i => !_searchresultItems.Any(x => x.ItemID == i.ItemID)));

            }

            return _searchresultItems;
        }
    }
}