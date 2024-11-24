using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class InvitacionProyectoBL
    {
        // MÉTODO PARA CREAR UNA NUEVA INVITACIÓN
        public async Task<int> EnviarInvitacionAsync(InvitacionProyecto invitacion)
        {
            // Verificar si el usuario ya está unido al proyecto
            var usuariosUnidos = await ProyectoUsuarioDAL.ObtenerUsuariosUnidosAsync(invitacion.IdProyecto);
            if (usuariosUnidos.Any(u => u.NombreUsuario == invitacion.CorreoElectronico))
            {
                return -1; // Usuario ya está unido al proyecto
            }

            // Verificar si ya existe una invitación pendiente
            var invitacionPendiente = await VerificarInvitacionPendiente(invitacion.CorreoElectronico, invitacion.IdProyecto);
            if (invitacionPendiente != null)
            {
                return -2; // Ya existe una invitación pendiente
            }

            return await InvitacionProyectoDAL.CrearInvitacionAsync(invitacion);
        }

        // MÉTODO PARA ACEPTAR UNA INVITACIÓN
        public async Task<int> AceptarInvitacionAsync(string token, int idUsuario, string correoUsuario)
        {
            // SE OBTIENE LA INVITACIÓN USANDO EL TOKEN
            var invitacion = await InvitacionProyectoDAL.ObtenerPorTokenAsync(token);
            if (invitacion != null)
            {
                // SE VERIFICA SI EL CORREO ELECTRÓNICO COINCIDE
                if (invitacion.CorreoElectronico != correoUsuario)
                {
                    return -2; // SI EL CORREO NO COINCIDE(CONTROLADOR)
                }

                // SE VERIFICA SI LA INVITACIÓN YA FUE PROCESADA
                if (invitacion.Estado != "Pendiente") // SI ES DIFERENTE A PENDIENTE 
                {
                    return -3; // YA FUE PROCESADA(CONTROLADOR)
                }

                // SE VERIFICA SI EL USUARIO YA ESTÁ UNIDO AL PROYECTO
                var usuariosUnidos = await ProyectoUsuarioDAL.ObtenerUsuariosUnidosAsync(invitacion.IdProyecto);
                if (usuariosUnidos.Any(u => u.Id == idUsuario))
                {
                    return -1; // SI YA PERTENECE A PROYECTO(CONTROLADOR)
                }
                // uNE AL USUARIO AL PROYECTO
                await ProyectoUsuarioDAL.UnirUsuarioAProyectoAsync(invitacion.IdProyecto, idUsuario);

                // SE ACTUALIZA EL ESTADO DE LA INVITACIÓN
                invitacion.Estado = "Aceptada"; // CAMBIA EL ESTADO A "ACEPTADA"
                invitacion.IdUsuario = idUsuario;
                return await InvitacionProyectoDAL.ActualizarInvitacionAsync(invitacion);
            }
            return 0; // RETORNA 0 SI NO SE ENCONTRÓ LA INVITACIÓN
        }


        // MÉTODO PARA RECHAZAR UNA INVITACIÓN
        public async Task<int> RechazarInvitacionAsync(string token, int idUsuario, string correoUsuario)
        {
            // SE OBTIENE LA INVITACIÓN USANDO EL TOKEN
            var invitacion = await InvitacionProyectoDAL.ObtenerPorTokenAsync(token);
            if (invitacion != null)
            {
                // SE VERIFICA SI EL CORREO ELECTRÓNICO COINCIDE
                if (invitacion.CorreoElectronico != correoUsuario)
                {
                    return -2; // SI EL CORREO NO COINCIDE(CONTROLADOR)
                }

                // Verificar si la invitación ya fue procesada
                if (invitacion.Estado != "Pendiente") // SI ES DIFERENTE A PENDIENTE 
                {
                    return -3; // YA FUE PROCESADA(CONTROLADOR)
                }

                invitacion.Estado = "Rechazada"; // CAMBIA EL ESTADO A "RECHAZADA"
                invitacion.IdUsuario = idUsuario; // SE REGISTRA EL ID DEL USUARIO QUE RECHAZÓ LA INVITACIÓN

                return await InvitacionProyectoDAL.ActualizarInvitacionAsync(invitacion);
            }
            return 0; // RETORNA 0 SI NO SE ENCONTRÓ LA INVITACIÓN
        }


        // MÉTODO PARA ELIMINAR UNA INVITACIÓN POR SU ID
        public async Task<bool> LimpiarInvitacionPorIdAsync(int id)
        {
            return await InvitacionProyectoDAL.EliminarInvitacionPorIdAsync(id);
        }


        // MÉTODO PARA OBTENER UNA INVITACIÓN POR TOKEN
        public async Task<InvitacionProyecto> ObtenerPorTokenAsync(string token)
        {
            return await InvitacionProyectoDAL.ObtenerPorTokenAsync(token);
        }


        // MÉTODO PARA OBTENER INVITACIONES POR ESTADO
        public async Task<List<InvitacionProyecto>> ObtenerInvitacionesPorEstadoAsync(int idProyecto, List<string> estados)
        {
            return await InvitacionProyectoDAL.ObtenerInvitacionesPorEstadoAsync(idProyecto, estados);
        }


        // MÉTODO PARA OBTENER INVITACIONES POR PROYECTO
        public async Task<List<InvitacionProyecto>> ObtenerInvitacionesPorProyectoAsync(int idProyecto)
        {
            return await InvitacionProyectoDAL.ObtenerInvitacionesPorProyectoAsync(idProyecto);
        }

        // Método para verificar si ya existe una invitación pendiente
        public async Task<InvitacionProyecto> VerificarInvitacionPendiente(string correoElectronico, int idProyecto)
        {
            return await InvitacionProyectoDAL.ObtenerInvitacionPendienteAsync(correoElectronico, idProyecto);
        }

    }
}
