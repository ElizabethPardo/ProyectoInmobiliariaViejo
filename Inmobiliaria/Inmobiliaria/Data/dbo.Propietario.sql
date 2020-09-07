CREATE TABLE [dbo].[Propietario] (
    [Id] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]        VARCHAR(50) NULL,
    [Apellido]      VARCHAR(50) NULL,
    [Dni]           VARCHAR(50) NULL,
    [Direccion]     VARCHAR(50) NULL,
    [Telefono]      VARCHAR(50) NULL,
    [Email]         VARCHAR(50) NULL,
    [Clave]         VARCHAR(50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

