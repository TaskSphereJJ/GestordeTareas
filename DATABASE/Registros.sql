	USE  GestorTareasReal
	GO
	INSERT INTO Cargo (Nombre) VALUES ('Administrador'), ('Colaborador'), ('Diseñador');

	-- Insertar datos en la tabla Categoria
	INSERT INTO Categoria (Nombre) VALUES ('Creación de informes'), ('Dibujo'), ('Redactar');

	-- Insertar datos en la tabla Prioridad
	INSERT INTO Prioridad (Nombre) VALUES ('Alta'), ('Media'), ('Baja');

	-- Insertar datos en la tabla EstadoTarea
	INSERT INTO EstadoTarea (Nombre) VALUES ('En espera'), ('En proceso'), ('Hecha');


	------insertar a las tablas que tienen foraneas
	-- Insertar datos en la tabla Usuarios
	INSERT INTO Usuarios (Nombre, Apellido, NombreUsuario, Email, Pass, Teléfono, FechaNacimiento, CargoId) 
	VALUES ('Jeffrey', 'Mardoqueo', 'JeffMardoqueo', 'jeffreymardoqueo260com', 'jeffreymardoqueo', '69842090', '1985-03-25', 1),
		   ('Juan', 'Pérez', 'juanperez', 'juan@example.com', 'password123', '123456789', '1990-05-15', 2)

	-- Insertar datos en la tabla Proyecto
	INSERT INTO Proyecto (Titulo, Descripcion, IdUsuario, CodigoAcceso, FechaFinalizacion) 
	VALUES (' Gestor Tareas ', 'Proyecto para llevar un control de atareas sijnasnkans', 4, '0987', '2024-06-30')


	-- Insertar datos en la tabla 
	INSERT INTO GrupoTrabajo (IdUsuario, IdUsuarioColabarador, IdProyecto)
	VALUES (1, 1, 2)

	-- Insertar datos en la tabla Tarea
	INSERT INTO Tarea (Nombre, Descripcion, FechaVencimiento, IdCategoria, IdPrioridad, IdEstadoTarea, ProyectoID, GrupoTrabajoID)
	VALUES ('Tarea 1', 'Descripción de la tarea 1', '2024-07-15', 1, 1, 1, 2, 4)

	-- Insertar datos en la tabla ElegirTarea
	INSERT INTO ElegirTarea (IdTarea, IdUsuario, FechaAsignacion, IdEstadoTarea)
	VALUES (3, 4, '2024-07-01', 1)

	-- Insertar datos en la tabla TareaFinalizada
INSERT INTO TareaFinalizada (IdElegirTarea, FechaFinalizacion, Comentarios)
VALUES (2, '2024-07-20', 'Comentarios de la tarea finalizada 1')

---- Insertar datos en la tabla ImagenesPruebas
--INSERT INTO ImagenesPruebas (Imagen, IdTareaFinalizada)
--VALUES ('imagen1.jpg', 1),
--       ('imagen2.jpg', 2);




select * from Cargo
select * from Categoria
select * from Prioridad
select * from Proyecto
select * from EstadoTarea
select * from Usuarios
select * from Proyecto
select * from Tarea
select * from TareaFinalizada
select * from GrupoTrabajo
select * from ElegirTarea
