CREATE TABLE [dbo].[Contrato] (
    [Id]          INT  IDENTITY (1, 1) NOT NULL,
    [CodContrato] INT  NULL,
    [FechaDesde]  DATE NULL,
    [FehaHasta]   DATE NULL,
    [InquilinoId] INT  NOT NULL,
    [InmuebleId]  INT  NOT NULL,
    [PagoId]      INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

