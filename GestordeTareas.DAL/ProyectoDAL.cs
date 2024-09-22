using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class ProyectoDAL
    {

        public static async Task<int> CreateAsync(Proyecto proyecto)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                ////proyecto.IdUsuario = idUsuario; // Asignar el IdUsuario al proyecto
                dbContext.Proyecto.Add(proyecto);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }


        public static async Task<int> UpdateAsync(Proyecto proyecto)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var existingProyecto = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);

                if (existingProyecto != null)
                {
                    existingProyecto.Titulo = proyecto.Titulo;
                    existingProyecto.Descripcion = proyecto.Descripcion;
                    existingProyecto.IdUsuario = proyecto.IdUsuario;
                    existingProyecto.FechaFinalizacion = proyecto.FechaFinalizacion;

                    dbContext.Update(existingProyecto);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<int> DeleteAsync(Proyecto proyecto)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var existingProyecto = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);
                if (existingProyecto != null)
                {
                    dbContext.Proyecto.Remove(existingProyecto);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<Proyecto> GetByIdAsync(Proyecto proyecto)
        {
            using (var bdContexto = new ContextoBD())
            {
                // Buscar el proyecto por su ID y cargar la propiedad de navegación Usuario si es necesario
                var projectBD = await bdContexto.Proyecto
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == proyecto.Id);

                // Manejar el caso cuando no se encuentra ningún proyecto
                if (projectBD == null)
                {
                    // Puedes lanzar una excepción, retornar null u otro valor según tu lógica de negocio
                    throw new Exception("El proyecto no existe en la base de datos.");
                }

                return projectBD;
            }
        }

        public static async Task<List<Proyecto>> GetAllAsync()
        {
            using (var dbContext = new ContextoBD())
            {
                var proyectos = await dbContext.Proyecto.Include(p => p.Usuario).ToListAsync();
                return proyectos;
            }
        }

        // Método para unir un usuario a un proyecto
        public static async Task<int> UnirUsuarioAProyectoAsync(int idProyecto, int idUsuario)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var proyectoUsuario = new ProyectoUsuario
                {
                    IdProyecto = idProyecto,
                    IdUsuario = idUsuario,
                    FechaAsignacion = DateTime.Now
                };

                dbContext.ProyectoUsuario.Add(proyectoUsuario);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }

        // Método para obtener los proyectos a los que un usuario se ha unido
        public static async Task<List<Proyecto>> ObtenerProyectosPorUsuarioAsync(int idUsuario)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.ProyectoUsuario
                    .Where(pu => pu.IdUsuario == idUsuario)
                    .Include(pu => pu.Proyecto) // Asegúrate de tener la relación configurada
                    .Select(pu => pu.Proyecto)
                    .ToListAsync();
            }
        }

        // Método para obtener proyectos disponibles para un usuario
        public static async Task<List<Proyecto>> ObtenerProyectosDisponiblesAsync(int idUsuario)
        {
            using (var dbContext = new ContextoBD())
            {
                var proyectosUnidos = await dbContext.ProyectoUsuario
                    .Where(pu => pu.IdUsuario == idUsuario)
                    .Select(pu => pu.IdProyecto)
                    .ToListAsync();

                return await dbContext.Proyecto
                    .Where(p => !proyectosUnidos.Contains(p.Id))
                    .ToListAsync();
            }
        }

        public static async Task<List<Usuario>> ObtenerUsuariosUnidosAsync(int idProyecto)
        {
            using (var context = new ContextoBD())
            {
                return await context.ProyectoUsuario
                    .Where(pu => pu.IdProyecto == idProyecto)
                    .Select(pu => pu.Usuario) // Asumiendo que ProyectoUsuario tiene una propiedad Usuario
                    .ToListAsync();
            }
        }

        public static async Task<int> EliminarUsuarioDeProyectoAsync(int idProyecto, int idUsuario)
        {
            int result = 0;

            using (var dbContext = new ContextoBD())
            {
                // Buscar el registro en la tabla ProyectoUsuario que asocia al usuario con el proyecto
                var proyectoUsuario = await dbContext.ProyectoUsuario
                    .FirstOrDefaultAsync(pu => pu.IdProyecto == idProyecto && pu.IdUsuario == idUsuario);

                if (proyectoUsuario != null)
                {
                    // Si se encuentra, eliminar el registro
                    dbContext.ProyectoUsuario.Remove(proyectoUsuario);
                    result = await dbContext.SaveChangesAsync(); // Guardar los cambios en la base de datos
                }
            }

            return result; // Retornar el número de registros afectados
        }



    }
}
