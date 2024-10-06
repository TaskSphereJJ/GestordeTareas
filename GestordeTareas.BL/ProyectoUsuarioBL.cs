using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class ProyectoUsuarioBL
    {
        public static async Task<int> UnirUsuarioAProyectoAsync(int idProyecto, int idUsuario)
        {
            // Llama al método en la capa DAL
            return await ProyectoUsuarioDAL.UnirUsuarioAProyectoAsync(idProyecto, idUsuario);
        }

        public static async Task<List<Proyecto>> ObtenerProyectosPorUsuarioAsync(int idUsuario)
        {
            // Llama al método en la capa DAL
            return await ProyectoUsuarioDAL.ObtenerProyectosPorUsuarioAsync(idUsuario);
        }

        public async Task<List<Usuario>> ObtenerUsuariosUnidosAsync(int idProyecto)
        {
            return await ProyectoUsuarioDAL.ObtenerUsuariosUnidosAsync(idProyecto);
        }

        public static async Task<int> EliminarUsuarioDeProyectoAsync(int idProyecto, int idUsuario)
        {
            // Llamar al método de ProyectoDAL para eliminar la relación
            return await ProyectoUsuarioDAL.EliminarUsuarioDeProyectoAsync(idProyecto, idUsuario);
        }

        // Método para verificar si un usuario es el encargado de un proyecto
        public async Task<bool> IsUsuarioEncargadoAsync(int idProyecto, int idUsuario)
        {
            return await ProyectoUsuarioDAL.IsUsuarioEncargadoAsync(idProyecto, idUsuario);
        }
    }
}
