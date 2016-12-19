namespace DatabaseAPI.DatabaseModel
{
    public class Item
    {
        public long ItemID { get; set; }
        public string Name { get; set; }
        public long ItemGroupID { get; set; }

        public Item(long itemId, string itemName, long itemGroupId)
        {
            ItemID = itemId;
            Name = itemName;
            ItemGroupID = itemGroupId;
        }

    }
}