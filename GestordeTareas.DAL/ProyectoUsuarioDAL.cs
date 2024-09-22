﻿using GestordeTaras.EN;
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