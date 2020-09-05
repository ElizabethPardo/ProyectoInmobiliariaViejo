CREATE TABLE [dbo].[Inmueble] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [DireccionInmueble] NVARCHAR (50) NULL,
    [Ambientes]         INT           NULL,
    [Uso]               INT           NULL,
    [Tipo]              INT           NULL,
    [Precio]            DECIMAL (18)  NULL,
    [Estado]            INT           NULL,
    [PropietarioId]     INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([PropietarioId]) REFERENCES [dbo].[Propietario] ([IdPropietario])
);


GO
