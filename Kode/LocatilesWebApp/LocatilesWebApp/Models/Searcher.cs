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
            // if searchstring contain space(s): split searchstring, keep all non-empty strings, add them to searchList
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
                else _searchList.Add(""); // if searchstring contain only spaces: add one empty string to searchList to return all Items in db

            }
            else _searchList.Add(searchString);

            // search db with each word in searchstring/searchList
            foreach (var s in _searchList) 
            {
                List<Item> _itemSearchWordResults = _db.TableItem.SearchItems(s);
                // add every item returned by search to final searchresult only once(searchresult will have no duplicates)
                _searchresultItems.AddRange(_itemSearchWordResults.FindAll(i => _searchresultItems.All(x => x.ItemID != i.ItemID)));  
            }
            return _searchresultItems;
        }
    }
}