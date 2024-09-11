---CREACION DE LA BD
CREATE DATABASE GestordeTareasBD
go
--usar la bd
USE GestorTareasBD
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
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
	NombreUsuario VARCHAR(50) NOT NULL,
    Pass VARCHAR(MAX) NOT NULL, -- Encriptar la contraseña
    Telefono VARCHAR(9) NOT NULL,
    FechaNacimiento DATE NOT NULL,
	[Status] INT NOT NULL,
	FechaRegistro DATETIME NOT NULL,
    IdCargo INT NOT NULL FOREIGN KEY REFERENCES Cargo(Id),

);

select * from usuario;
GO
-- Creación del proyecto
CREATE TABLE Proyecto (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Titulo VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
	FechaFinalizacion DATE NOT NULL,
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(Id),
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
-- Tabla de tarea finalizada
CREATE TABLE TareaFinalizada (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    FechaFinalizacion DATE NOT NULL DEFAULT GETDATE(),
    Comentarios VARCHAR(MAX) NOT NULL,
    IdElegirTarea INT NOT NULL FOREIGN KEY REFERENCES ElegirTarea(Id),
);

GO

-- Tabla de Imagenes de Pruebas
CREATE TABLE ImagenesPrueba (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Imagen VARCHAR(MAX) NOT NULL,
    IdTareaFinalizada INT NOT NULL FOREIGN KEY REFERENCES TareaFinalizada(Id)
);

GO

