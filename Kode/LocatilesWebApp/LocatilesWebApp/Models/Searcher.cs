using System.Collections.Generic;
using System.Linq;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace LocatilesWebApp.Models
{
    public class Searcher : ISearcher
    {

        private IDatabaseService _db;

        public Searcher(IDatabaseService databaseService = null)
        {
            _db = databaseService == null ? new DatabaseService(new SqlStoreDatabaseFactory()) : databaseService;
        }


        public List<Item> Search(string searchString)
        {
            List<string> _searchList = new List<string>();
            List<Item> _searchresultItems = new List<Item>();
            if (searchString.ToLower().Contains(' '))
            {
                string[] sa = searchString.Split(' ');
                if(sa.Any(str => str != ""))
                {
                    sa = sa.Where(str => str != "").ToArray();
                    for (int s = 0; s < sa.Length; s++)
                    {
                        _searchList.Add(sa[s]);
                    }

                }
                else _searchList.Add("");

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