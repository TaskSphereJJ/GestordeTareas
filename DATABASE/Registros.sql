-- USE GestorTareasJ

-- SELECT * FROM Cargo
-- GO
-- SELECT * FROM Prioridad
-- GO
-- SELECT * FROM Categoria
-- GO

-- -----INSERTAR A TABLAS
-- INSERT INTO EstadoTarea(Nombre) VALUES ('Pendiente')
-- SELECT * FROM EstadoTarea
-- GO

-- -- Tabla Usuarios
-- INSERT INTO Usuarios (Nombre, Apellido, Email, Pass, Telefono, FechaNacimiento, CargoId ) 
-- VALUES ('Jeffrey', 'Mardoqueo', 'jeffreymardoqueo260@gmail.com','jeffrey20068f', '69842090', '2006-08-02', 1);
-- GO

-- INSERT INTO Usuarios (Nombre, Apellido, Email, Pass, Telefono, FechaNacimiento, CargoId ) 
-- VALUES ('David', 'Hernandez', 'david0@gmail.com','david', '12345678', '2003-02-16', 2);
-- GO


---CREACION DE LA BD
CREATE DATABASE GestordeTareasBD
--usar la bd
USE GestordeTareasBD
--------------------------------------------TABLAS
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
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY (1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Pass VARCHAR(100) NOT NULL, -- Encriptar la contraseña
    Teléfono VARCHAR(20),
    FechaNacimiento DATE,
    CargoId INT NOT NULL FOREIGN KEY REFERENCES Cargo(Id)
);
GO
-- Tabla para almacenar los administradores
CREATE TABLE Administradores (
    ID INT PRIMARY KEY IDENTITY (1,1),
    UsuarioID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Usuarios(Id),
    Contraseña VARCHAR(100) NOT NULL -- Encriptar la contraseña
);
GO
-- Tabla para almacenar los colaboradores
CREATE TABLE Colaboradores (
    ID INT PRIMARY KEY IDENTITY (1,1),
    UsuarioID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Usuarios(Id),
    Contraseña VARCHAR(100) NOT NULL -- Encriptar la contraseña
);
GO
-- Creación del proyecto
CREATE TABLE Proyecto (
    Id INT PRIMARY KEY IDENTITY (1,1),
    Titulo VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
    AdministradorID INT NOT NULL FOREIGN KEY REFERENCES Administradores(Id),
    CodigoAcceso VARCHAR(100) UNIQUE, -- Código de acceso único para el proyecto
    FechaFinalizacion DATE
);
GO
-- Grupo de trabajo: El administrador y el colaborador del proyecto
CREATE TABLE GrupoTrabajo (
    Id INT PRIMARY KEY IDENTITY (1,1), -- Identificador único del grupo de trabajo
    AdministradorId INT NOT NULL FOREIGN KEY REFERENCES Administradores(Id),
    ColaboradorID INT NOT NULL FOREIGN KEY REFERENCES Colaboradores(ID),
    ProyectoID INT NOT NULL FOREIGN KEY REFERENCES Proyecto(Id),
    CONSTRAINT UQ_AdminColProyecto UNIQUE (AdministradorID, ColaboradorID, ProyectoID)
);
GO
-- Tarea creada por el administrador
CREATE TABLE Tarea (
    ID INT PRIMARY KEY IDENTITY (1,1),
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
    IdTarea INT NOT NULL FOREIGN KEY REFERENCES Tarea(ID),
    IdColaborador INT NOT NULL FOREIGN KEY REFERENCES Colaboradores(ID),
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

-- Insertar datos en la tabla Cargo
INSERT INTO Cargo (Nombre) VALUES ('Gerente'), ('Desarrollador'), ('Diseñador');

-- Insertar datos en la tabla Categoria
INSERT INTO Categoria (Nombre) VALUES ('Creación de informes'), ('Otro'), ('Ejemplo');

-- Insertar datos en la tabla Prioridad
INSERT INTO Prioridad (Nombre) VALUES ('Alta'), ('Media'), ('Baja');

-- Insertar datos en la tabla EstadoTarea
INSERT INTO EstadoTarea (Nombre) VALUES ('En espera'), ('En proceso'), ('Hecha');

-- Insertar datos en la tabla Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, Pass, Teléfono, FechaNacimiento, CargoId) 
VALUES ('Juan', 'Pérez', 'juan@example.com', 'password123', '123456789', '1990-05-15', 1),
       ('María', 'López', 'maria@example.com', 'password456', '987654321', '1995-10-20', 2),
       ('Carlos', 'Gómez', 'carlos@example.com', 'password789', '456789123', '1985-03-25', 3);

-- Insertar datos en la tabla Administradores
INSERT INTO Administradores (UsuarioID, Contraseña) VALUES (1, 'adminpassword');

-- Insertar datos en la tabla Colaboradores
INSERT INTO Colaboradores (UsuarioID, Contraseña) VALUES (2, 'colaborador1password'), 
                                                        (3, 'colaborador2password'), 
                                                        (4, 'colaborador3password');

-- Insertar datos en la tabla Proyecto
INSERT INTO Proyecto (Titulo, Descripcion, AdministradorID, CodigoAcceso, FechaFinalizacion) 
VALUES ('Proyecto 1', 'Descripción del proyecto 1', 1, 'ABC123', '2024-12-31'),
       ('Proyecto 2', 'Descripción del proyecto 2', 1, 'DEF456', '2024-11-30'),
       ('Proyecto 3', 'Descripción del proyecto 3', 1, 'GHI789', '2024-10-31');

-- Insertar datos en la tabla GrupoTrabajo
INSERT INTO GrupoTrabajo (AdministradorId, ColaboradorID, ProyectoID) 
VALUES (1, 2, 1), (1, 3, 1), (1, 4, 1);

-- Insertar datos en la tabla Tarea
INSERT INTO Tarea (Nombre, Descripcion, FechaVencimiento, IdCategoria, IdPrioridad, IdEstadoTarea, ProyectoID, GrupoTrabajoID)
VALUES ('Tarea 1', 'Descripción de la tarea 1', '2024-12-31', 1, 1, 1, 1, 1),
       ('Tarea 2', 'Descripción de la tarea 2', '2024-11-30', 2, 2, 2, 1, 1),
       ('Tarea 3', 'Descripción de la tarea 3', '2024-10-31', 3, 3, 3, 1, 1);

-- Insertar datos en la tabla ElegirTarea
INSERT INTO ElegirTarea (IdTarea, IdColaborador, IdEstadoTarea) 
VALUES (1, 2, 1), (2, 3, 2), (3, 4, 1);

-- Insertar datos en la tabla TareaFinalizada
INSERT INTO TareaFinalizada (IdElegirTarea, FechaFinalizacion, Comentarios) 
VALUES (1, '2024-12-31', 'Tarea 1 completada correctamente'),
       (2, '2024-11-30', 'Tarea 2 finalizada a tiempo'),
       (3, '2024-10-31', 'Tarea 3 completada por el colaborador');

-- Insertar datos en la tabla ImagenesPruebas
INSERT INTO ImagenesPruebas (Imagen, IdTareaFinalizada) 
VALUES ('ruta/a/imagen1.jpg', 1),
       ('ruta/a/imagen2.jpg', 2),
       ('ruta/a/imagen3.jpg', 3);

select * from Cargo
select * from Categoria
select * from Prioridad
select * from Proyecto
select * from EstadoTarea
select * from Colaboradores
select * from ImagenesPruebas
select * from Tarea
select * from TareaFinalizada
select * from GrupoTrabajo
select * from Administradores
select * from ElegirTarea
select * from Usuarios