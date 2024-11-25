using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class ProyectoUsuarioDAL
    {
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
                    .Include(pu => pu.Proyecto) 
                    .Select(pu => pu.Proyecto)
                    .ToListAsync();
            }
        }
           
        public static async Task<List<Usuario>> ObtenerUsuariosUnidosAsync(int idProyecto)
        {
            using (var context = new ContextoBD())
            {
                return await context.ProyectoUsuario
                    .Where(pu => pu.IdProyecto == idProyecto)
                    .Select(pu => pu.Usuario) 
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

        // Método para asignar un usuario como encargado de un proyecto
        public static async Task<bool> AsignarEncargadoAsync(int idProyecto, int idUsuarioNuevoEncargado)
        {
            using (var dbContext = new ContextoBD())
            {
                // Verificar si ya existe un encargado para este proyecto
                var encargadoExistente = await dbContext.ProyectoUsuario
                    .FirstOrDefaultAsync(pu => pu.IdProyecto == idProyecto && pu.Encargado);

                // Si ya existe un encargado, no se puede asignar otro
                if (encargadoExistente != null)
                {
                    return false; // Ya existe un encargado para este proyecto
                }

                // Buscar el registro para el nuevo encargado
                var nuevoEncargado = await dbContext.ProyectoUsuario
                    .FirstOrDefaultAsync(pu => pu.IdProyecto == idProyecto && pu.IdUsuario == idUsuarioNuevoEncargado);

                if (nuevoEncargado != null)
                {
                    // Asignar como encargado
                    nuevoEncargado.Encargado = true;
                    await dbContext.SaveChangesAsync(); // Guardar los cambios
                    return true; // Asignación exitosa
                }

                return false; // No se encontró el usuario para asignar como encargado
            }
        }

        // Método para verificar si un usuario es el encargado de un proyecto
        public static async Task<bool> IsUsuarioEncargadoAsync(int idProyecto, int idUsuario)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.ProyectoUsuario
                    .AnyAsync(pu => pu.IdProyecto == idProyecto && pu.IdUsuario == idUsuario && pu.Encargado);
            }
        }

        public static async Task<Usuario> ObtenerEncargadoPorProyectoAsync(int idProyecto)
        {
            using (var dbContext = new ContextoBD())
            {
                // Buscar el encargado del proyecto
                var encargado = await dbContext.ProyectoUsuario
                    .Where(pu => pu.IdProyecto == idProyecto && pu.Encargado)
                    .Select(pu => pu.Usuario) 
                    .FirstOrDefaultAsync();

                return encargado; // Retorna el usuario encargado o null si no existe
            }
        }

        public static async Task<List<ProyectoUsuario>> ObtenerTodosAsync()
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.ProyectoUsuario
                    .Include(pu => pu.Proyecto)  // Incluye los proyectos asociados
                    .Include(pu => pu.Usuario)   // Incluye los usuarios asociados
                    .ToListAsync();              // Devuelve todos los registros
            }
        }

    }
}
