# Gestor de Tareas

Este proyecto es un sistema para llevar el control de tareas y asignarlas a diferentes roles o trabajadores. El sistema cuenta con una parte de administración y una parte para los trabajadores, implementado con el patrón de arquitectura en N-Capas y MVC en .NET Core.

![Diagrama de Arquitectura en N-Capas](https://github.com/JeffreyMardoqueo-17/Gestor-de-Tareas/assets/126411958/79c4469e-1a3b-4225-909d-02463d87762e)

## Arquitectura en N-Capas

### EN (Entidades)

- **Usuario**
- **Tarea**
- **Prioridad**
- **IniciarSesion**
- **ImagenTarea**
- **Categoria**
- **Cargo**
- **AsignacionTarea**

![Diagrama de Entidades](https://github.com/JeffreyMardoqueo-17/Gestor-de-Tareas/assets/126411958/2f00bfba-33d3-4a07-b66f-48882c7edcb6)

### DAL (Acceso a Datos)
En esta capa se maneja el acceso a la base de datos SQL Server utilizando Entity Framework Core. Aquí se definen los contextos y los repositorios para interactuar con las entidades:

- **ContextoBD**: Define el contexto de la base de datos y contiene los DbSet para cada entidad.

#### Detalles de las Entidades:
- **Usuario**: Representa a un usuario del sistema, con información como nombre, correo electrónico y rol.
- **Tarea**: Representa una tarea a realizar, con descripción, fecha de vencimiento y estado.
- **Prioridad**: Define la prioridad de una tarea, con propiedades como nombre y nivel de prioridad.
- **IniciarSesion**: Representa la información de inicio de sesión de un usuario, con propiedades como nombre de usuario y contraseña.
- **ImagenTarea**: Contiene la ruta de la imagen asociada a una tarea.
- **Categoria**: Define la categoría a la que pertenece una tarea, con propiedades como nombre y descripción.
- **Cargo**: Representa el cargo o rol de un trabajador en la organización.
- **AsignacionTarea**: Relaciona a un usuario con una tarea asignada, incluyendo información como fecha de asignación y estado de la asignación.

### BL (Lógica de Negocio)
En esta capa se encuentra la lógica de negocio de la aplicación, incluyendo operaciones como la asignación de tareas, la gestión de prioridades, etc. Aquí se definen las clases de lógica de negocio para cada entidad, como `AsignacionTareasBL`, `CargoBL`, `CategoriaBL`, etc.

### UI (Interfaz de Usuario)
En esta capa se encuentra la interfaz de usuario del sistema, implementada con ASP.NET Core MVC y vistas Razor. Se utiliza Bootstrap para el diseño y la maquetación de las vistas.

## Tecnologías Utilizadas
- **Lenguaje**: C#
- **Framework**: .NET Core
- **Base de Datos**: SQL Server
- **ORM**: Entity Framework Core

## NuGets Utilizados
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.VisualStudio.Web.CodeGeneration.Design
## LA BASE DE DAROS ESTARIA ASI:
1. **Cargo**:
   - Id
   - Nombre

2. **Categoria**:
   - Id
   - Nombre

3. **Prioridad**:
   - Id
   - Nombre

4. **EstadoTarea**:
   - Id
   - Nombre

5. **Usuarios**:
   - Id
   - Nombre
   - Apellido
   - Email
   - Pass
   - Teléfono
   - FechaNacimiento
   - CargoId

6. **Administradores**:
   - ID
   - UsuarioID

7. **Colaboradores**:
   - ID
   - UsuarioID

8. **Proyecto**:
   - Id
   - Descripcion
   - AdministradorID
   - CodigoAcceso
   - FechaFinalizacion

9. **GrupoTrabajo**:
   - Id
   - AdministradorId
   - ColaboradorID
   - ProyectoID
   - UQ_AdminColProyecto

10. **Tarea**:
    - ID
    - Nombre
    - Descripcion
    - FechaVencimiento
    - FechaCreacion
    - IdCategoria
    - IdPrioridad
    - IdEstadoTarea
    - ProyectoID
    - GrupoTrabajoID

11. **ElejirTarea**:
    - Id
    - IdTarea
    - IdColaborador
    - FechaAsignacion
    - IdEstadoTarea

12. **TareaFinalizada**:
    - Id
    - IdElejirTarea
    - FechaFinalizacion
    - Comentarios
    - ImagenesDePrueba

13. **ImagenTareaFinalizada**
   -Id
   --Imagen
   --IdTaresFinalizada
