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
CREATE TABLE Usuario (
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
	NombreUsuario VARCHAR(50) NOT NULL,
    Pass VARCHAR(MAX)  NULL, -- Encriptar la contraseña
    Telefono VARCHAR(9) NOT NULL,
    FechaNacimiento DATE NOT NULL,
	[Status] TINYINT NOT NULL,
	FechaRegistro DATETIME NOT NULL,
    IdCargo INT NOT NULL FOREIGN KEY REFERENCES Cargo(Id),

);

--ALTER TABLE Usuario
--ALTER COLUMN [Status] TINYINT NOT NULL;

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

CREATE TABLE ProyectoUsuario(
	Id INT PRIMARY KEY IDENTITY (1,1),
	IdProyecto INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
	IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuario(Id),
	FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
	Encargado BIT NULL
)

Select * from ProyectoUsuario

update ProyectoUsuario
set Encargado = 1 where id=12
-- Insertar datos en la tabla Cargo
INSERT INTO Cargo (Nombre) VALUES ('Administrador'), ('Colaborador'), ('Encargado');

-- Insertar datos en la tabla Categoria
INSERT INTO Categoria (Nombre) VALUES ('Informes'), ('Desarrollo'), ('Gestión');

-- Insertar datos en la tabla Prioridad
INSERT INTO Prioridad (Nombre) VALUES ('Baja'), ('Media'), ('Alta');

-- Insertar datos en la tabla EstadoTarea
INSERT INTO EstadoTarea (Nombre) VALUES ('Pendiente'), ('En Proceso'), ('Finalizada');
select * from EstadoTarea

-- Insertar datos en la tabla Usuario
INSERT INTO Usuario (Nombre, Apellido, NombreUsuario, Pass, Telefono, FechaNacimiento, FechaRegistro, [Status], IdCargo)
VALUES 
  ('lester', 'serrano', 'lesterserrano66@gmail.com', '827ccb0eea8a706c4c34a16891f84e7b', '123456789', '1990-01-01', GETDATE(), 1, 1);
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

  INSERT INTO Tarea (Nombre, Descripcion, FechaCreacion, FechaVencimiento, IdCategoria, IdPrioridad, IdEstadoTarea, IdProyecto)
VALUES 
  ('Tarea 1', 'Descripción de la Tarea 1', GETDATE(), '2024-12-31', 1, 1, 1, 1);

-- Insertar datos en la tabla ElegirTarea
INSERT INTO ElegirTarea (FechaAsignacion, IdTarea, IdUsuario, IdProyecto)
VALUES 
  (GETDATE(), 1, 1, 1),
  (GETDATE(), 2, 1, 2),
  (GETDATE(), 3, 1, 3);

-- Insertar datos en la tabla TareaFinalizada
INSERT INTO TareaFinalizada (FechaFinalizacion, Comentarios, IdElegirTarea)
VALUES 
  (GETDATE(), 'Tarea finalizada correctamente', 1),
  (GETDATE(), 'Tarea finalizada con éxito', 2),
  (GETDATE(), 'Tarea completada', 3);

-- Insertar datos en la tabla ImagenesPrueba
INSERT INTO ImagenesPrueba (Imagen, IdTareaFinalizada)
VALUES 
  ('ruta/imagen1.jpg', 1),
  ('ruta/imagen2.jpg', 2),
  ('ruta/imagen3.jpg', 3);