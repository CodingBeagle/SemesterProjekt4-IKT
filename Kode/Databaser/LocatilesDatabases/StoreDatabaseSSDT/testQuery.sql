insert into ItemGroup (Name)
values ('Mejeri');

insert into ItemGroup (Name)
values ('Diverse');

select * from ItemGroup;

delete from ItemGroup where ItemGroupID=3;

delete from Floorplan;
delete from Item;
delete from ItemGroup;
delete from ItemSectionPlacement;
delete from StoreSection;

select * from Floorplan;
select * from Item;
select * from ItemGroup;
select * from ItemSectionPlacement;
select * from StoreSection;