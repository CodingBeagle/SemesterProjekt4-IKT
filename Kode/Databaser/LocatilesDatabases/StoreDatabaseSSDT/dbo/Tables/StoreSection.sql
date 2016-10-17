CREATE TABLE [dbo].[StoreSection] (
    [StoreSectionID] BIGINT NOT NULL IDENTITY(1,1),
    [Name]           NVARCHAR(450)   NOT NULL UNIQUE,
    [CoordinateX]    BIGINT NOT NULL,
    [CoordinateY]    BIGINT NOT NULL,
    [FloorPlanID]    BIGINT NOT NULL,
    CONSTRAINT [pk_StoreSection] PRIMARY KEY CLUSTERED ([StoreSectionID] ASC),
    CONSTRAINT [fk_StoreSection] FOREIGN KEY ([FloorPlanID]) REFERENCES [dbo].[Floorplan] ([FloorPlanID]) ON DELETE CASCADE ON UPDATE CASCADE,
    UNIQUE NONCLUSTERED ([StoreSectionID] ASC)
);

