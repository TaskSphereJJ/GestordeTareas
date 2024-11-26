---CREACION DE LA BD
CREATE DATABASE GestordeTareasBD
go
--usar la bd
USE GestordeTareasBD
go
----------TABLAS
-- Cargo: Para saber si es administrador o colaborador
CREATE TABLE Cargo(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

GO
-- Categoria: Para saber si es de creación de informes o así
CREATE TABLE Categoria(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

GO
-- Prioridad: Para saber si es alta, media o baja
CREATE TABLE Prioridad(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

GO
-- Estado tarea: Para saber si está en espera, proceso o hecha
CREATE TABLE EstadoTarea(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

GO

-- Tabla Usuarios
CREATE TABLE Usuario (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    FotoPerfil NVARCHAR(MAX) NOT NULL,
	Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
	NombreUsuario VARCHAR(50) NOT NULL,
    Pass VARCHAR(MAX) NOT NULL, 
    Telefono VARCHAR(9) NOT NULL,
    FechaNacimiento DATE NOT NULL,
	[Status] TINYINT NOT NULL,
	FechaRegistro DATETIME NOT NULL,
    IdCargo INT NOT NULL FOREIGN KEY REFERENCES Cargo(Id),

);

GO


-- Creación del proyecto
CREATE TABLE Proyecto (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Titulo VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
	CodigoAcceso NVARCHAR(50) NOT NULL UNIQUE,
	FechaFinalizacion DATE NOT NULL,
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(Id),
	--CONSTRAINT UQ_CodigoAcceso UNIQUE (CodigoAcceso)
);

GO


-- Tarea creada por el administrador
CREATE TABLE Tarea (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaVencimiento DATETIME NOT NULL,
    IdCategoria INT NOT NULL FOREIGN KEY REFERENCES Categoria(Id),
    IdPrioridad INT NOT NULL FOREIGN KEY REFERENCES Prioridad(Id),
    IdEstadoTarea INT NOT NULL FOREIGN KEY REFERENCES EstadoTarea(Id),
    IdProyecto INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
);


GO


-- Cuando se asignan las tareas y le aparecen al colaborador, elegirá una
CREATE TABLE ElegirTarea (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    IdTarea INT NOT NULL FOREIGN KEY REFERENCES Tarea(Id),
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(Id),
   	IdProyecto INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
);


GO

--Tabla que hace la relacion de unir un usuario a un proyecto especifico
CREATE TABLE ProyectoUsuario(
	Id INT PRIMARY KEY IDENTITY (1,1),
	IdProyecto INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(Id),
	FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
	Encargado BIT NULL
);

GO

select * from ProyectoUsuario

--Tabla de invitacion de usuario a proyecto especifico y dependiendo de la respuesta lo une al proyecto osea hace un ProyectoUsuario
CREATE TABLE InvitacionProyecto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdProyecto INT NOT NULL,
    IdUsuario INT NULL,
    CorreoElectronico NVARCHAR(255) NOT NULL,
    Estado NVARCHAR(20) NOT NULL, 
    Token NVARCHAR(255) NOT NULL UNIQUE, 
    FechaCreacion DATETIME NOT NULL,
    FechaExpiracion DATETIME NOT NULL,
    CONSTRAINT FK_InvitacionProyecto_Proyecto FOREIGN KEY (IdProyecto) REFERENCES Proyecto(Id),
    CONSTRAINT FK_InvitacionProyecto_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_InvitacionProyecto_Correo_Proyecto UNIQUE (CorreoElectronico, IdProyecto) -- Se asegura que un mismo correo no pueda recibir varias invitaciones al mismo proyecto
);

--ON DELETE CASCADE

--ALTER TABLE InvitacionProyecto
--DROP CONSTRAINT FK_InvitacionProyecto_Usuario;  -- Elimina la clave foránea actual

--ALTER TABLE InvitacionProyecto
--ADD CONSTRAINT FK_InvitacionProyecto_Usuario 
--    FOREIGN KEY (IdUsuario) 
--    REFERENCES Usuario(Id)
--    ON DELETE CASCADE;  -- Configura la eliminación en cascada



GO

CREATE TABLE PasswordResetCode (
    Id INT IDENTITY(1,1) PRIMARY KEY,              
    Codigo NVARCHAR(50) NOT NULL,                       
    Expiration DATETIME NOT NULL,             
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(Id)
);

-- Tabla comentario
CREATE TABLE Comment (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Content NVARCHAR(MAX) NOT NULL,
    FechaComentario DATETIME NOT NULL,
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(Id),
	IdProyecto INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id)
);

delete  from Comment


-- Tabla de tarea finalizada
--CREATE TABLE TareaFinalizada (
--    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
--    FechaFinalizacion DATE NOT NULL DEFAULT GETDATE(),
--    Comentarios VARCHAR(MAX) NOT NULL,
--    IdElegirTarea INT NOT NULL FOREIGN KEY REFERENCES ElegirTarea(Id),
--);

--GO

---- Tabla de Imagenes de Pruebas
--CREATE TABLE ImagenesPrueba (
--    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
--    Imagen VARCHAR(MAX) NOT NULL,
--    IdTareaFinalizada INT NOT NULL FOREIGN KEY REFERENCES TareaFinalizada(Id)
--);


--GO

--INSERT PARA LAS TABLAS CREADAS

-- Insertar datos en la tabla Cargo
INSERT INTO Cargo (Nombre) VALUES ('Administrador'), ('Colaborador');

-- Insertar datos en la tabla Categoria
INSERT INTO Categoria (Nombre) VALUES ('Informes'), ('Desarrollo'), ('Gestión');

-- Insertar datos en la tabla Prioridad
INSERT INTO Prioridad (Nombre) VALUES ('Baja'), ('Media'), ('Alta');

-- Insertar datos en la tabla EstadoTarea
INSERT INTO EstadoTarea (Nombre) VALUES ('Pendiente'), ('En Proceso'), ('Finalizada');

-- Insertar datos en la tabla Usuario (Usuario administrador de ejemplo)
INSERT INTO Usuario (Nombre, Apellido, NombreUsuario, Pass, Telefono, FechaNacimiento, FechaRegistro, [Status], IdCargo)
VALUES 
  ('lester', 'serrano', 'lester@gmail.com', '827ccb0eea8a706c4c34a16891f84e7b', '123456789', '1990-01-01', GETDATE(), 1, 1);
  --La contraseña de este usuario es 12345

-- Insertar datos en la tabla Proyecto
INSERT INTO Proyecto (Titulo, Descripcion, FechaFinalizacion, IdUsuario)
VALUES 
  ('Proyecto A', 'Descripción del Proyecto A', '2024-12-31', 1),
  ('Proyecto B', 'Descripción del Proyecto B', '2025-03-31', 1),
  ('Proyecto C', 'Descripción del Proyecto C', '2025-06-30', 1);

  -- Insertar datos en la tabla Tarea
INSERT INTO Tarea (Nombre, Descripcion, FechaCreacion, FechaVencimiento, IdCategoria, IdPrioridad, IdEstadoTarea, IdProyecto)
VALUES 
  ('Tarea 1', 'Descripción de la Tarea 1', GETDATE(), '2024-12-31', 1, 1, 1, 1),
  ('Tarea 2', 'Descripción de la Tarea 2', GETDATE(), '2025-03-31', 2, 2, 1, 2),
  ('Tarea 3', 'Descripción de la Tarea 3', GETDATE(), '2025-06-30', 3, 3, 1, 3);

-- Insertar datos en la tabla ElegirTarea
--INSERT INTO ElegirTarea (FechaAsignacion, IdTarea, IdUsuario, IdProyecto)
--VALUES 
--  (GETDATE(), 1, 1, 1),
--  (GETDATE(), 2, 1, 2),
--  (GETDATE(), 3, 1, 3);
