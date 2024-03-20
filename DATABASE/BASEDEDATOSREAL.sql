
-- Para saber si es administrador o colaborador

CREATE DATABASE GestorTareasReal
GO 
USE GestorTareasReal


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
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
	NombreUsuario VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Pass VARCHAR(100) NOT NULL, -- Encriptar la contraseña
    Teléfono VARCHAR(20),
    FechaNacimiento DATE,
    CargoId INT NOT NULL FOREIGN KEY REFERENCES Cargo(Id)
);
GO
-- Creación del proyecto
CREATE TABLE Proyecto (
    Id INT PRIMARY KEY IDENTITY (1,1),
    Titulo VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    CodigoAcceso VARCHAR(100) UNIQUE, -- Código de acceso único para el proyecto
    FechaFinalizacion DATE
);
GO
-- Grupo de trabajo: El administrador y el colaborador del proyecto
CREATE TABLE GrupoTrabajo (
    Id INT PRIMARY KEY IDENTITY (1,1), -- Identificador único del grupo de trabajo
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    IdUsuarioColabarador INT NOT NULL FOREIGN KEY REFERENCES Usuarios(ID),
    IdProyecto INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
    CONSTRAINT UQ_AdminColProyecto UNIQUE (IdUsuario, IdUsuarioColabarador, IdProyecto)
);
GO
-- Tarea creada por el administrador
CREATE TABLE Tarea (
    Id INT PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
    FechaVencimiento DATE NOT NULL,
    FechaCreacion DATE NOT NULL DEFAULT GETDATE(),
    IdCategoria INT NOT NULL FOREIGN KEY REFERENCES Categoria(Id),
    IdPrioridad INT NOT NULL FOREIGN KEY REFERENCES Prioridad(Id),
    IdEstadoTarea INT NOT NULL FOREIGN KEY REFERENCES EstadoTarea(Id),
    ProyectoID INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
    GrupoTrabajoID INT NOT NULL FOREIGN KEY REFERENCES GrupoTrabajo(Id)
);
GO
-- Cuando se asignan las tareas y le aparecen al colaborador, elegirá una
CREATE TABLE ElegirTarea (
    Id INT PRIMARY KEY IDENTITY (1,1),
    IdTarea INT NOT NULL FOREIGN KEY REFERENCES Tarea(Id),
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    FechaAsignacion DATE NOT NULL DEFAULT GETDATE(),
    IdEstadoTarea INT NOT NULL FOREIGN KEY REFERENCES EstadoTarea(Id)
);
GO
-- Tabla de tarea finalizada
CREATE TABLE TareaFinalizada (
    Id INT PRIMARY KEY IDENTITY (1,1),
    IdElegirTarea INT NOT NULL FOREIGN KEY REFERENCES ElegirTarea(Id),
    FechaFinalizacion DATE NOT NULL DEFAULT GETDATE(),
    Comentarios VARCHAR(MAX)
);
GO
-- Tabla de Imagenes de Pruebas
CREATE TABLE ImagenesPruebas (
    Id INT PRIMARY KEY IDENTITY (1,1),
    Imagen VARCHAR(MAX),
    IdTareaFinalizada INT NOT NULL FOREIGN KEY REFERENCES TareaFinalizada(Id)
);
GO
