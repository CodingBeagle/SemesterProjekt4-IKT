CREATE TABLE [dbo].[Item] (
    [ItemID]      BIGINT NOT NULL IDENTITY(1,1),
    [Name]        TEXT   NOT NULL,
    [ItemGroupID] BIGINT NOT NULL,
    CONSTRAINT [pk_Item] PRIMARY KEY CLUSTERED ([ItemID] ASC),
    CONSTRAINT [fk_Item] FOREIGN KEY ([ItemGroupID]) REFERENCES [dbo].[ItemGroup] ([ItemGroupID]) ON UPDATE CASCADE,
    UNIQUE NONCLUSTERED ([ItemID] ASC)
);

