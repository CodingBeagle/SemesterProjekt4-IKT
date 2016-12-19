CREATE TABLE [dbo].[StoreSection] (
    [StoreSectionID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (450) NOT NULL,
    [CoordinateX]    BIGINT         NOT NULL,
    [CoordinateY]    BIGINT         NOT NULL,
    [FloorPlanID]    BIGINT         NOT NULL,
    UNIQUE NONCLUSTERED ([StoreSectionID] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [pk_StoreSection] PRIMARY KEY CLUSTERED ([StoreSectionID] ASC),
    CONSTRAINT [fk_StoreSection] FOREIGN KEY ([FloorPlanID]) REFERENCES [dbo].[Floorplan] ([FloorPlanID]) ON DELETE CASCADE ON UPDATE CASCADE
);

