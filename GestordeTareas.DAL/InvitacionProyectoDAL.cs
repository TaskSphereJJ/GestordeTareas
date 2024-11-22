using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class InvitacionProyectoDAL
    {
        // MÉTODO PARA CREAR UNA NUEVA INVITACIÓN
        public static async Task<int> CrearInvitacionAsync(InvitacionProyecto invitacion)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                dbContext.InvitacionProyecto.Add(invitacion);
                result = await dbContext.SaveChangesAsync();
            }
            return result; // Retornar el número de registros afectados
        }


        // MÉTODO PARA OBTENER UNA INVITACIÓN POR TOKEN
        public static async Task<InvitacionProyecto> ObtenerPorTokenAsync(string token)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.InvitacionProyecto
                    .FirstOrDefaultAsync(i => i.Token == token);
            }
        }

        // MÉTODO PARA ACTUALIZAR UNA INVITACIÓN
        public static async Task<int> ActualizarInvitacionAsync(InvitacionProyecto invitacion)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                dbContext.InvitacionProyecto.Update(invitacion);
                result = await dbContext.SaveChangesAsync();
            }
            return result; // RETORNA EL NÚMERO DE REGISTROS AFECTADOS
        }

        // MÉTODO PARA OBTENER INVITACIONES EXPIRADAS
        public static async Task<List<InvitacionProyecto>> ObtenerInvitacionesRechazadasAsync()
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.InvitacionProyecto
                .Where(i => i.Estado == "Rechazada" || i.FechaExpiracion < DateTime.UtcNow)
                .ToListAsync();
            }
        }

        // MÉTODO PARA ELIMINAR INVITACIONES POR ID
        public static async Task<bool> EliminarInvitacionPorIdAsync(int id)
        {
            using (var dbContext = new ContextoBD())
            {
                // SE OBTIENEN LAS INVITACIONES POR SU ID
                var invitacion = await dbContext.InvitacionProyecto
            .FirstOrDefaultAsync(i => i.Id == id);

                // Si se encuentra la invitación, se elimina
                if (invitacion != null)
                {
                    dbContext.InvitacionProyecto.Remove(invitacion);
                    await dbContext.SaveChangesAsync(); // Guardar cambios
                    return true; // Retornar verdadero si se eliminó con éxito
                }
            }
            return false; 
        }


        // MÉTODO PARA FILTRAR LAS INVITACIONES POR ESTADO
        public static async Task<List<InvitacionProyecto>> ObtenerInvitacionesPorEstadoAsync(int idProyecto, List<string> estados)
        {
            using (var dbContext = new ContextoBD())
            {
                // Filtrar las invitaciones según el estado proporcionado
                return await dbContext.InvitacionProyecto
                    .Where(i => i.IdProyecto == idProyecto && estados.Contains(i.Estado))
                    .ToListAsync();
            }
        }

        // MÉTODO PARA OBTENER INVITACIONES POR PROYECTO
        public static async Task<List<InvitacionProyecto>> ObtenerInvitacionesPorProyectoAsync(int idProyecto)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.InvitacionProyecto
                    .Where(i => i.IdProyecto == idProyecto)
                    .ToListAsync();
            }
        }

        // MÉTODO PARA VERIFICAR SI YA EXISTE UNA INVITACIÓN PENDIENTE PARA UN CORREO Y UN PROYECTO
        public static async Task<InvitacionProyecto> ObtenerInvitacionPendienteAsync(string correoElectronico, int idProyecto)
        {
            using (var dbContext = new ContextoBD())
            {
                // Buscar una invitación pendiente para el correo y el proyecto proporcionado
                return await dbContext.InvitacionProyecto
                    .FirstOrDefaultAsync(i => i.CorreoElectronico == correoElectronico && i.IdProyecto == idProyecto && i.Estado == "Pendiente");
            }
        }



    }
}
