using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class ProyectoBL
    {
        public async Task<int> CreateAsync(Proyecto proyecto)
        {
            return await ProyectoDAL.CreateAsync(proyecto);
        }
        public async Task<int> UpdateAsync(Proyecto proyecto)
        {
            return await ProyectoDAL.UpdateAsync(proyecto);
        }
        public async Task<int> DeleteAsync(Proyecto proyecto)
        {
            return await ProyectoDAL.DeleteAsync(proyecto);
        }
        public async Task<Proyecto> GetById(Proyecto proyecto)
        {
            return await ProyectoDAL.GetByIdAsync(proyecto);
        }

        public async Task<List<Proyecto>> GetAllAsync()
        {
            return await ProyectoDAL.GetAllAsync();
        }

        public static async Task<int> UnirUsuarioAProyectoAsync(int idProyecto, int idUsuario)
        {
            // Llama al método en la capa DAL
            return await ProyectoDAL.UnirUsuarioAProyectoAsync(idProyecto, idUsuario);
        }

        public static async Task<List<Proyecto>> ObtenerProyectosPorUsuarioAsync(int idUsuario)
        {
            // Llama al método en la capa DAL
            return await ProyectoDAL.ObtenerProyectosPorUsuarioAsync(idUsuario);
        }

        public static async Task<List<Proyecto>> ObtenerProyectosDisponiblesAsync(int idUsuario)
        {
            // Llama al método en la capa DAL
            return await ProyectoDAL.ObtenerProyectosDisponiblesAsync(idUsuario);
        }

        public async Task<List<Usuario>> ObtenerUsuariosUnidosAsync(int idProyecto)
        {
            return await ProyectoDAL.ObtenerUsuariosUnidosAsync(idProyecto);
        }

        public static async Task<int> EliminarUsuarioDeProyectoAsync(int idProyecto, int idUsuario)
        {
            // Llamar al método de ProyectoDAL para eliminar la relación
            return await ProyectoDAL.EliminarUsuarioDeProyectoAsync(idProyecto, idUsuario);
        }


    }
}