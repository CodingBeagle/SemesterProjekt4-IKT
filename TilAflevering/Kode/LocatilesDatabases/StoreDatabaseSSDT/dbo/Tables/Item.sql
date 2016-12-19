CREATE TABLE [dbo].[Item] (
    [ItemID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (450) NOT NULL,
    [ItemGroupID] BIGINT         NOT NULL,
    UNIQUE NONCLUSTERED ([ItemID] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [pk_Item] PRIMARY KEY CLUSTERED ([ItemID] ASC),
    CONSTRAINT [fk_Item] FOREIGN KEY ([ItemGroupID]) REFERENCES [dbo].[ItemGroup] ([ItemGroupID]) ON UPDATE CASCADE
);

