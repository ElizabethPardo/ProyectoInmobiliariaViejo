CREATE TABLE [dbo].[Pago] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
	[NroPago]    iNT           NULL,
    [FechaPago]  DATE         NULL,
    [Importe]    DECIMAL (18) NULL,
    [ContratoId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ContratoId]) REFERENCES [dbo].[Contrato] ([Id])
);

