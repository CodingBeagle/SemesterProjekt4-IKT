using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace LocatilesWebApp.Models
{
    public interface ISearcher
    {
        List<Item> Search(string searchString);
    }
}