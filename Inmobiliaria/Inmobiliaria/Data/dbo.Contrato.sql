CREATE TABLE [dbo].Contrato
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [CodContrato] INT NULL, 
    [FechaDesde] DATE NULL, 
    [FechaHasta] DATE NULL, 
    [InquilinoId] INT NOT NULL, 
    [InmuebleId] INT NOT NULL, 
    [PagoId] INT NOT NULL
)
