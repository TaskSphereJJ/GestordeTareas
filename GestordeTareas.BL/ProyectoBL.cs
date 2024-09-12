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
    }
}