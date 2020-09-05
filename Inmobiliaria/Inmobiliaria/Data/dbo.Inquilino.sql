CREATE TABLE [dbo].[Inquilino] (
    [IdInquilino]     INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]          NVARCHAR (50) NULL,
    [Apellido]        NVARCHAR (50) NULL,
    [Dni]             NVARCHAR (50) NULL,
    [Telefono]        NVARCHAR (50) NULL,
    [Direccion]       NVARCHAR (50) NULL,
    [Email]           NVARCHAR (50) NULL,
    [LugarTrabajo]    NVARCHAR (50) NULL,
    [NombreGarante]   NVARCHAR (50) NULL,
    [ApellidoGarante] NVARCHAR (50) NULL,
    [DniGarante]      NVARCHAR (50) NULL,
    [TelefonoGarante] NVARCHAR (50) NULL,
    [DireccionGarante]    NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([IdInquilino] ASC)
);

