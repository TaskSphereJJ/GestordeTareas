using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Método para obtener los usuarios unidos a un proyecto
        public static async Task<List<Usuario>> ObtenerUsuariosUnidosAsync(int idProyecto)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.ProyectoUsuario
                    .Where(pu => pu.IdProyecto == idProyecto)
                    .Select(pu => pu.Usuario)
                    .ToListAsync();
            }
        }

        // Método para eliminar la unión de un usuario con un proyecto
        public static async Task<int> EliminarUsuarioDeProyectoAsync(int idProyecto, int idUsuario)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var proyectoUsuario = await dbContext.ProyectoUsuario
                    .FirstOrDefaultAsync(pu => pu.IdProyecto == idProyecto && pu.IdUsuario == idUsuario);

                if (proyectoUsuario != null)
                {
                    dbContext.ProyectoUsuario.Remove(proyectoUsuario);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        // Método para asignar un usuario como encargado de un proyecto
        public static async Task<bool> AsignarEncargadoAsync(int idProyecto, int idUsuarioNuevoEncargado)
        {
            using (var dbContext = new ContextoBD())
            {
                var encargadoExistente = await dbContext.ProyectoUsuario
                    .FirstOrDefaultAsync(pu => pu.IdProyecto == idProyecto && pu.Encargado);

                if (encargadoExistente != null)
                    return false; // Ya existe un encargado

                var nuevoEncargado = await dbContext.ProyectoUsuario
                    .FirstOrDefaultAsync(pu => pu.IdProyecto == idProyecto && pu.IdUsuario == idUsuarioNuevoEncargado);

                if (nuevoEncargado != null)
                {
                    nuevoEncargado.Encargado = true;
                    await dbContext.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }

        // Método para verificar si un usuario es encargado de un proyecto
        public static async Task<bool> IsUsuarioEncargadoAsync(int idProyecto, int idUsuario)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.ProyectoUsuario
                    .AnyAsync(pu => pu.IdProyecto == idProyecto && pu.IdUsuario == idUsuario && pu.Encargado);
            }
        }

        // Método para obtener el encargado de un proyecto
        public static async Task<Usuario> ObtenerEncargadoPorProyectoAsync(int idProyecto)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.ProyectoUsuario
                    .Where(pu => pu.IdProyecto == idProyecto && pu.Encargado)
                    .Select(pu => pu.Usuario)
                    .FirstOrDefaultAsync();
            }
        }

        // Método para obtener todos los registros de ProyectoUsuario
        public static async Task<List<ProyectoUsuario>> ObtenerTodosAsync()
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.ProyectoUsuario
                    .Include(pu => pu.Proyecto)
                    .Include(pu => pu.Usuario)
                    .ToListAsync();
            }
        }
    }
}
