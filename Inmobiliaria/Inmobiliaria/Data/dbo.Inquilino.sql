CREATE TABLE [dbo].[Inquilino] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]           VARCHAR(50) NULL,
    [Apellido]         VARCHAR(50) NULL,
    [Dni]              VARCHAR(50) NULL,
    [Telefono]         VARCHAR(50) NULL,
    [Direccion]        VARCHAR(50) NULL,
    [Email]            VARCHAR(50) NULL,
    [LugarTrabajo]     VARCHAR(50) NULL,
    [NombreGarante]    VARCHAR(50) NULL,
    [ApellidoGarante]  NVARCHAR (50) NULL,
    [DniGarante]       VARCHAR(50) NULL,
    [TelefonoGarante]  VARCHAR(50) NULL,
    [DireccionGarante] VARCHAR(50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

