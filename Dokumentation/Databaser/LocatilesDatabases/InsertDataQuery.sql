/* ------Reset all tables------*/
/* delete from Floorplan; */
delete from ItemSectionPlacement;
delete from StoreSection;
delete from Item;
delete from ItemGroup;


/*
DBCC CHECKIDENT ('[Floorplan]',RESEED, 0);
DBCC CHECKIDENT ('[ItemSectionPlacement]',RESEED, 0);
DBCC CHECKIDENT ('[StoreSection]',RESEED, 0);
*/
DBCC CHECKIDENT ('[Item]',RESEED, 0);
DBCC CHECKIDENT ('[ItemGroup]',RESEED, 0);


/* ------Inserts testdata------*/
/* ItemGroup*/
insert into ItemGroup(Name) values (ItemGroupA);
insert into ItemGroup(Name) values (ItemGroupB);
insert into ItemGroup(Name) values (ItemGroupC);
insert into ItemGroup(Name) values (ItemGroupD);
insert into ItemGroup(Name) values (SubGroupD,4);

/* Items*/
insert into Item(Name, ItemGroupID) values (ItemA1, 1);
insert into Item(Name, ItemGroupID) values (ItemA2, 1);
insert into Item(Name, ItemGroupID) values (ItemA3, 1);
insert into Item(Name, ItemGroupID) values (ItemA4, 1);

insert into Item(Name, ItemGroupID) values (ItemB1, 2);
insert into Item(Name, ItemGroupID) values (ItemB2, 2);
insert into Item(Name, ItemGroupID) values (ItemB3, 2);
insert into Item(Name, ItemGroupID) values (ItemB4, 2);

insert into Item(Name, ItemGroupID) values (ItemC1, 3);
insert into Item(Name, ItemGroupID) values (ItemC2, 3);
insert into Item(Name, ItemGroupID) values (ItemC3, 3);
insert into Item(Name, ItemGroupID) values (ItemC4, 3);

insert into Item(Name, ItemGroupID) values (ItemD1, 5);
insert into Item(Name, ItemGroupID) values (ItemD2, 5);
insert into Item(Name, ItemGroupID) values (ItemD3, 5);
insert into Item(Name, ItemGroupID) values (ItemD4, 5);

/*
/* Floorplan*/
insert into Floorplan(Name, imageHeight, imageWidth, Image) select 'Floorplan1', 1000, 1000, BulkColumn from Openrowset( Bulk 'image_path_here', Single_Blob) as Image;

/* StoreSection*/
insert into StoreSection(Name,CoordinateX,CoordinateY,FloorPlanID) values (StoreSectionA,100,100,1);
insert into StoreSection(Name,CoordinateX,CoordinateY,FloorPlanID) values (StoreSectionB,200,200,1);
insert into StoreSection(Name,CoordinateX,CoordinateY,FloorPlanID) values (StoreSectionC,300,300,1);
insert into StoreSection(Name,CoordinateX,CoordinateY,FloorPlanID) values (StoreSectionD,400,400,1);

/* ItemSectionPlacement*/
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (1,1);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (2,1);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (3,1);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (4,1);

insert into ItemSectionPlacement(ItemID, StoreSectionID) values (6,2);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (7,2);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (8,2);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (9,2);

insert into ItemSectionPlacement(ItemID, StoreSectionID) values (10,3);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (11,3);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (12,3);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (13,3);

insert into ItemSectionPlacement(ItemID, StoreSectionID) values (14,4);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (15,4);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (16,4);
insert into ItemSectionPlacement(ItemID, StoreSectionID) values (17,4);
*/
