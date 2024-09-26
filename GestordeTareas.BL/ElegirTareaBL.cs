using GestordeTaras.EN;
using GestordeTareas.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TuNamespace;

namespace GestordeTareas.BL
{
    public class ElegirTareaBL
    {
        public async Task<bool> ElegirTareaAsync(int idTarea, int idUsuario, int idProyecto)
        {
            // Verificar si los parámetros son válidos
            if (idTarea <= 0)
                throw new ArgumentException("El ID de la tarea debe ser mayor que cero.", nameof(idTarea));

            if (idUsuario <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor que cero.", nameof(idUsuario));

            if (idProyecto <= 0)
                throw new ArgumentException("El ID del proyecto debe ser mayor que cero.", nameof(idProyecto));

            var elegirTarea = new ElegirTarea
            {
                IdTarea = idTarea,
                IdUsuario = idUsuario,
                IdProyecto = idProyecto,
                FechaAsignacion = DateTime.Now
            };

            // Llamar al método DAL para realizar la asignación de la tarea
            return await ElegirTareaDAL.ElegirTareaAsyncDAL(elegirTarea);
        }
    }
}
