USE GestorTareasJ

SELECT * FROM Cargo
GO
SELECT * FROM Prioridad
GO
SELECT * FROM Categoria
GO

-----INSERTAR A TABLAS
INSERT INTO EstadoTarea(Nombre) VALUES ('Pendiente')
SELECT * FROM EstadoTarea
GO

-- Tabla Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, Pass, Telefono, FechaNacimiento, CargoId ) 
VALUES ('Jeffrey', 'Mardoqueo', 'jeffreymardoqueo260@gmail.com','jeffrey20068f', '69842090', '2006-08-02', 1);
GO

INSERT INTO Usuarios (Nombre, Apellido, Email, Pass, Telefono, FechaNacimiento, CargoId ) 
VALUES ('David', 'Hernandez', 'david0@gmail.com','david', '12345678', '2003-02-16', 2);
GO



