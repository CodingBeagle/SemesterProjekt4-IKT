CREATE TABLE [dbo].[Floorplan] (
    [Name]        NVARCHAR (450) NOT NULL,
    [FloorPlanID] BIGINT         NOT NULL,
    [Image]       IMAGE          NOT NULL,
    [imageWidth]  BIGINT         NOT NULL,
    [imageHeight] BIGINT         NOT NULL,
    UNIQUE NONCLUSTERED ([FloorPlanID] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [pk_Floorplan] PRIMARY KEY CLUSTERED ([FloorPlanID] ASC)
);


