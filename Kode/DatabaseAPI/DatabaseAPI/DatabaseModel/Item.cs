namespace DatabaseAPI.DatabaseModel
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int ItemGroupID { get; set; }

        public Item(int itemId, string itemName, int itemGroupId)
        {
            ItemID = itemId;
            Name = itemName;
            ItemGroupID = itemGroupId;
        }

    }
}