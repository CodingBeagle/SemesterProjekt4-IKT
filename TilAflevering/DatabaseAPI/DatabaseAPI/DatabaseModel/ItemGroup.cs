namespace DatabaseAPI.DatabaseModel
{
    public class ItemGroup
    {
        public string ItemGroupName { get; set; }
        public long ItemGroupParentID { get; set; }
        public long ItemGroupID { get;  set; }

        public ItemGroup( string itemGroupName , long itemGroupParentID , long itemGroupID )
        {
            ItemGroupName = itemGroupName;
            ItemGroupParentID = itemGroupParentID;
            ItemGroupID = itemGroupID;
        }
    }
}