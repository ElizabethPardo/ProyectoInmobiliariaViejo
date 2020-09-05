CREATE TABLE [dbo].[Propietario] (
    [IdPropietario]        INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]    NVARCHAR (50) NULL,
    [Apellido]  NVARCHAR (50) NULL,
    [Dni]       NVARCHAR (50) NULL,
    [Direccion] NVARCHAR (50) NULL,
    [Telefono]  NVARCHAR (50) NULL,
    [Email]     NVARCHAR (50) NULL,
    [Clave]     NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([IdPropietario] ASC)
);

