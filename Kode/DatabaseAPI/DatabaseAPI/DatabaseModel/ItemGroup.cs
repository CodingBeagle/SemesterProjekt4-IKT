namespace DatabaseAPI.DatabaseModel
{
    public class ItemGroup
    {
        public string ItemGroupName { get; private set; }
        public long ItemGroupParentID { get; private set; }
        public int ItemGroupID { get; private set; }

        public ItemGroup( string itemGroupName , long itemGroupParentID , int itemGroupID )
        {
            ItemGroupName = itemGroupName;
            ItemGroupParentID = itemGroupParentID;
            ItemGroupID = itemGroupID;
        }
    }
}