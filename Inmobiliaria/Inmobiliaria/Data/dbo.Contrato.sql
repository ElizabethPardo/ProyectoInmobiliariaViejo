CREATE TABLE [dbo].[Contrato] (
    [Id]          INT IDENTITY (1, 1) NOT NULL, 
    [FechaDesde]  DATE NULL,
    [FechaHasta]  DATE NULL,
    [InquilinoId] INT  NOT NULL,
    [InmuebleId]  INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([InquilinoId]) REFERENCES [dbo].[Inquilino] ([Id]),
    FOREIGN KEY ([InmuebleId]) REFERENCES [dbo].[Inmueble] ([Id])
);

