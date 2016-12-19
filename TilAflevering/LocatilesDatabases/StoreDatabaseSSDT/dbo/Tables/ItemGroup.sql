CREATE TABLE [dbo].[ItemGroup] (
    [ItemGroupID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (450) NOT NULL,
    [ParentItemGroupID] BIGINT         NULL,
    UNIQUE NONCLUSTERED ([ItemGroupID] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [pk_ItemGroup] PRIMARY KEY CLUSTERED ([ItemGroupID] ASC),
    CONSTRAINT [fk_ItemGroup] FOREIGN KEY ([ParentItemGroupID]) REFERENCES [dbo].[ItemGroup] ([ItemGroupID])
);

