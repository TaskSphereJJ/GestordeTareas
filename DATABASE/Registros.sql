
-- Insertar datos en la tabla Cargo
INSERT INTO Cargo (Nombre) VALUES ('Administrador'), ('Colaborador'), ('Supervisor');

-- Insertar datos en la tabla Categoria
INSERT INTO Categoria (Nombre) VALUES ('Informes'), ('Desarrollo'), ('Gestión');

-- Insertar datos en la tabla Prioridad
INSERT INTO Prioridad (Nombre) VALUES ('Baja'), ('Media'), ('Alta');

-- Insertar datos en la tabla EstadoTarea
INSERT INTO EstadoTarea (Nombre) VALUES ('Pendiente'), ('En Proceso'), ('Finalizada');

-- Insertar datos en la tabla Usuario
INSERT INTO Usuario (Nombre, Apellido, NombreUsuario, Pass, Telefono, FechaNacimiento, FechaRegistro, [Status], IdCargo)
VALUES 
  ('lester', 'serrano', 'lesterserrano', '827ccb0eea8a706c4c34a16891f84e7b', '123456789', '1990-01-01', GETDATE(), 1, 1);
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
