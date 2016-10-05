namespace DatabaseAPI.DatabaseModel
{
    public class ItemGroup
    {
        public string ItemGroupName { get; private set; }
        public long ItemGroupParentID { get; private set; }
        public long ItemGroupID { get; private set; }

        public ItemGroup( string itemGroupName , long itemGroupParentID , long itemGroupID )
        {
            ItemGroupName = itemGroupName;
            ItemGroupParentID = itemGroupParentID;
            ItemGroupID = itemGroupID;
        }
    }
}