using DatabaseAPI.Factories;

namespace DatabaseAPI.DatabaseModel
{
    public class ItemSectionPlacement
    {
        public Item  Item{ get; set; }
        public StoreSectionModel Section { get; set; }

        private DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
        private long _itemID;
        private long _sectionID;
        public ItemSectionPlacement(long itemID, long sectionID, long itemId, long sectionId)
        {
            _itemID = itemId;
            _sectionID = sectionId;
        }

        //Gets the item and section information from the server
        public void UpdateInfo()
        {
            Item = db.TableItem.GetItem(_itemID);
            Section = db.TableStoreSection.GetStoreSection(_sectionID);
        }
    }
}