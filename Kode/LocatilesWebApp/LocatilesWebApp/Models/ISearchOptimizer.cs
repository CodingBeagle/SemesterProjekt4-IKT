using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace LocatilesWebApp.Models
{
    public interface ISearchOptimizer
    {
        List<Item> SearchOptimization(string searchString);
    }
}