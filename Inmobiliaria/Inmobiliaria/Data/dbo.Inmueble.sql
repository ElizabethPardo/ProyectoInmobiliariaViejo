CREATE TABLE [dbo].[Inmueble] (
    [Id]            INT NOT NULL IDENTITY PRIMARY KEY,
    [Ambientes]     INT          NULL,
    [Uso]           INT          NULL,
    [Tipo]          INT          NULL,
    [Precio]        DECIMAL (18) NULL,
    [Estado]        BIT          NULL,
    [PropietarioId] INT          NOT NULL,
    FOREIGN KEY ([PropietarioId]) REFERENCES [dbo].[Propietario] ([Id])
);

