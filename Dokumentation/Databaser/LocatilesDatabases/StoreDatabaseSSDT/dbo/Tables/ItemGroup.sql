CREATE TABLE [dbo].[ItemGroup] (
    [ItemGroupID]  BIGINT NOT NULL,
    [Name]         TEXT   NOT NULL,
    [rItemGroupID] BIGINT NULL,
    CONSTRAINT [pk_ItemGroup] PRIMARY KEY CLUSTERED ([ItemGroupID] ASC),
    CONSTRAINT [fk_ItemGroup] FOREIGN KEY ([rItemGroupID]) REFERENCES [dbo].[ItemGroup] ([ItemGroupID]),
    UNIQUE NONCLUSTERED ([ItemGroupID] ASC)
);

