using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItemGroup
{
    public interface ITableItemGroup
    {
        void CreateItemGroup(string itemGroupItem, int itemGroupParentID);
        void DeleteItemGroup(int itemGroupID);
        ItemGroup GetItemGroup(int itemGroupID);
    }
}