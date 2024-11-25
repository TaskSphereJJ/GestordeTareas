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
        public async Task<int> UnirUsuarioAProyectoAsync(int idProyecto, int idUsuario)
        {
            // Llama al método en la capa DAL
            return await ProyectoUsuarioDAL.UnirUsuarioAProyectoAsync(idProyecto, idUsuario);
        }

        public async Task<List<Proyecto>> ObtenerProyectosPorUsuarioAsync(int idUsuario)
        {
            // Llama al método en la capa DAL
            return await ProyectoUsuarioDAL.ObtenerProyectosPorUsuarioAsync(idUsuario);
        }

        public async Task<List<Usuario>> ObtenerUsuariosUnidosAsync(int idProyecto)
        {
            return await ProyectoUsuarioDAL.ObtenerUsuariosUnidosAsync(idProyecto);
        }

        public async Task<int> EliminarUsuarioDeProyectoAsync(int idProyecto, int idUsuario)
        {
            // Llamar al método de ProyectoDAL para eliminar la relación
            return await ProyectoUsuarioDAL.EliminarUsuarioDeProyectoAsync(idProyecto, idUsuario);
        }

        // Método para asignar un usuario como encargado de un proyecto
        public async Task<bool> AsignarEncargadoAsync(int idProyecto, int idUsuarioNuevoEncargado)
        {
            // Llama al método en la capa DAL
            return await ProyectoUsuarioDAL.AsignarEncargadoAsync(idProyecto, idUsuarioNuevoEncargado);
        }

        // Método para verificar si un usuario es el encargado de un proyecto
        public async Task<bool> IsUsuarioEncargadoAsync(int idProyecto, int idUsuario)
        {
            return await ProyectoUsuarioDAL.IsUsuarioEncargadoAsync(idProyecto, idUsuario);
        }

        // Método para obtener el encargado de un proyecto
        public async Task<Usuario> ObtenerEncargadoPorProyectoAsync(int idProyecto)
        {
            return await ProyectoUsuarioDAL.ObtenerEncargadoPorProyectoAsync(idProyecto);
        }

        public async Task<List<ProyectoUsuario>> ObtenerTodosAsync()
        {
            return await ProyectoUsuarioDAL.ObtenerTodosAsync();
        }


    }
}
