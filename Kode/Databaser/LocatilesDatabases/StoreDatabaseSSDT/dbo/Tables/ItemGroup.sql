CREATE TABLE [dbo].[ItemGroup] (
    [ItemGroupID]  BIGINT NOT NULL IDENTITY(1,1),
    [Name]         NVARCHAR(450)   NOT NULL UNIQUE,
    [ParentItemGroupID] BIGINT NULL,
    CONSTRAINT [pk_ItemGroup] PRIMARY KEY CLUSTERED ([ItemGroupID] ASC),
    CONSTRAINT [fk_ItemGroup] FOREIGN KEY ([ParentItemGroupID]) REFERENCES [dbo].[ItemGroup] ([ItemGroupID]),
    UNIQUE NONCLUSTERED ([ItemGroupID] ASC)
);

