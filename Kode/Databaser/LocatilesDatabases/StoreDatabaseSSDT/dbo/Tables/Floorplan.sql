CREATE TABLE [dbo].[Floorplan] (
    [Name]        TEXT   NOT NULL,
    [FloorPlanID] BIGINT NOT NULL,
    [Image]       IMAGE  NOT NULL,
    [imageWidth]  BIGINT NOT NULL,
    [imageHeight] BIGINT NOT NULL,
    CONSTRAINT [pk_Floorplan] PRIMARY KEY CLUSTERED ([FloorPlanID] ASC),
    UNIQUE NONCLUSTERED ([FloorPlanID] ASC)
);

