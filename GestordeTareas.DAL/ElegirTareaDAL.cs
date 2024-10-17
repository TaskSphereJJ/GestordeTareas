using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuNamespace;

namespace GestordeTareas.DAL
{
    public static class ElegirTareaDAL
    {
        public static async Task<bool> ElegirTareaAsyncDAL(ElegirTarea elegirTarea)
        {
            if (elegirTarea == null)
                throw new ArgumentNullException(nameof(elegirTarea), "El objeto ElegirTarea no puede ser nulo.");

            try
            {
                using (var dbContexto = new ContextoBD())
                {
                    await dbContexto.ElegirTarea.AddAsync(elegirTarea);
                    int result = await dbContexto.SaveChangesAsync();
                    return result > 0;
                }
            }
            catch (DbUpdateException)
            {
                return false; // Manejo de errores
            }
            catch (Exception)
            {
                return false; // Manejo de errores
            }
        }

        // Método para verificar si la tarea ya fue elegida por el usuario
        public static async Task<bool> TareaYaElegidaAsync(int idTarea, int idUsuario)
        {
            using (var dbContexto = new ContextoBD())
            {
                return await dbContexto.ElegirTarea.AnyAsync(et => et.IdTarea == idTarea && et.IdUsuario == idUsuario);
            }
        }
        public static async Task<List<Tarea>> ObtenerTareasElegidasPorUsuarioAsync(int idUsuario, int idProyecto)
        {
            using (var dbContext = new ContextoBD())
            {
                var tareas = await dbContext.ElegirTarea
                    .Include(et => et.Tarea)
                        .ThenInclude(t => t.Categoria)
                    .Include(et => et.Tarea)
                        .ThenInclude(t => t.Prioridad)
                    .Include(et => et.Tarea)
                        .ThenInclude(t => t.EstadoTarea)
                    .Include(et => et.Tarea)
                        .ThenInclude(t => t.Proyecto)
                    .Where(et => et.IdUsuario == idUsuario && et.IdProyecto == idProyecto)
                    .Select(et => et.Tarea)
                    .ToListAsync();

                return tareas;
            }
        }

    }
}