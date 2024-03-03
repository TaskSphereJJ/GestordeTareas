---CREACION DE LA BD
CREATE DATABASE GestorTareas
--usar la bd
USE GestorTareas
--------------------------------------------TABLAS
-- Cargo: Para saber si es administrador o colaborador
CREATE TABLE Cargo(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

-- Categoria: Para saber si es de creación de informes o así
CREATE TABLE Categoria(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

-- Prioridad: Para saber si es alta, media o baja
CREATE TABLE Prioridad(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

-- Estado tarea: Para saber si está en espera, proceso o hecha
CREATE TABLE EstadoTarea(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR (50) NOT NULL
);

-- Tabla Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Pass VARCHAR(100) NOT NULL, -- Encriptar la contraseña
    Teléfono VARCHAR(20),
    FechaNacimiento DATE,
    CargoId INT NOT NULL FOREIGN KEY REFERENCES Cargo(Id)
);

-- Tabla para almacenar los administradores
CREATE TABLE Administradores (
    ID INT PRIMARY KEY IDENTITY,
    UsuarioID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Usuarios(Id),
    Contraseña VARCHAR(100) NOT NULL -- Encriptar la contraseña
);

-- Tabla para almacenar los colaboradores
CREATE TABLE Colaboradores (
    ID INT PRIMARY KEY IDENTITY,
    UsuarioID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Usuarios(Id),
    Contraseña VARCHAR(100) NOT NULL -- Encriptar la contraseña
);

-- Creación del proyecto
CREATE TABLE Proyecto (
    Id INT PRIMARY KEY IDENTITY,
    Titulo VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
    AdministradorID INT NOT NULL FOREIGN KEY REFERENCES Administradores(Id),
    CodigoAcceso VARCHAR(100) UNIQUE, -- Código de acceso único para el proyecto
    FechaFinalizacion DATE
);

-- Grupo de trabajo: El administrador y el colaborador del proyecto
CREATE TABLE GrupoTrabajo (
    Id INT PRIMARY KEY IDENTITY, -- Identificador único del grupo de trabajo
    AdministradorId INT NOT NULL FOREIGN KEY REFERENCES Administradores(Id),
    ColaboradorID INT NOT NULL FOREIGN KEY REFERENCES Colaboradores(ID),
    ProyectoID INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
    CONSTRAINT UQ_AdminColProyecto UNIQUE (AdministradorID, ColaboradorID, ProyectoID)
);

-- Tarea creada por el administrador
CREATE TABLE Tarea (
    ID INT PRIMARY KEY IDENTITY,
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

-- Cuando se asignan las tareas y le aparecen al colaborador, elegirá una
CREATE TABLE ElegirTarea (
    Id INT PRIMARY KEY IDENTITY,
    IdTarea INT NOT NULL FOREIGN KEY REFERENCES Tarea(ID),
    IdColaborador INT NOT NULL FOREIGN KEY REFERENCES Colaboradores(ID),
    FechaAsignacion DATE NOT NULL DEFAULT GETDATE(),
    IdEstadoTarea INT NOT NULL FOREIGN KEY REFERENCES EstadoTarea(Id)
);

-- Tabla de tarea finalizada
CREATE TABLE TareaFinalizada (
    Id INT PRIMARY KEY IDENTITY,
    IdElegirTarea INT NOT NULL FOREIGN KEY REFERENCES ElegirTarea(Id),
    FechaFinalizacion DATE NOT NULL DEFAULT GETDATE(),
    Comentarios VARCHAR(MAX),
    ImagenesDePrueba VARCHAR(MAX)
);
