CREATE TABLE [dbo].[Usuario] (
    [Id]       INT    IDENTITY (1, 1) NOT NULL,
    [Nombre]   VARCHAR (50)  NULL,
    [Apellido] VARCHAR (50)  NULL,
    [Email]    VARCHAR (50)  NULL,
    [Clave]    VARCHAR (MAX) NULL,
    [Rol]      INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

