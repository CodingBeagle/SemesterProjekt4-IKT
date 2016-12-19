using System.Collections.Generic;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItemGroup
{
    public interface ITableItemGroup
    {
        long CreateItemGroup(string itemGroupItem, long itemGroupParentID);
        long CreateItemGroup(string itemGroupItemName);
        void DeleteItemGroup(long itemGroupID);
        ItemGroup GetItemGroup(long itemGroupID);
        List<ItemGroup> SearchItemGroups(string itemGroupName);
        void UpdateItemGroup(string oldItemGroupName, string newItemGroupName);
        List<ItemGroup> GetAllItemGroups();
    }
}