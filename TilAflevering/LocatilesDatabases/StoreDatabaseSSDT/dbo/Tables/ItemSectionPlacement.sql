CREATE TABLE [dbo].[ItemSectionPlacement] (
    [ItemID]         BIGINT NOT NULL,
    [StoreSectionID] BIGINT NOT NULL,
    CONSTRAINT [pk_ItemSectionPlacement] PRIMARY KEY CLUSTERED ([ItemID] ASC, [StoreSectionID] ASC),
    CONSTRAINT [fk_ItemSectionPlacement2] FOREIGN KEY ([StoreSectionID]) REFERENCES [dbo].[StoreSection] ([StoreSectionID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [fk_ItemSectionPlacement] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[Item] ([ItemID]) ON DELETE CASCADE ON UPDATE CASCADE
);
