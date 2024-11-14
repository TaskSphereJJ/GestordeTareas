using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class PrioridadBL
    {
        public async Task<int> CreateAsync(Prioridad prioridad)
        {
            return await PrioridadDAL.CreateAsync(prioridad);
        }
        public async Task<int> UpdateAsync(Prioridad prioridad)
        {
            return await PrioridadDAL.UpdateAsync(prioridad);
        }
        public async Task<int> DeleteAsync(Prioridad prioridad)
        {
            return await PrioridadDAL.DeleteAsync(prioridad);
        }
        public async Task<Prioridad> GetById(Prioridad prioridad)
        {
            return await PrioridadDAL.GetByIdAsync(prioridad);
        }
        public async Task<List<Prioridad>> GetAllAsync()
        {
            return await PrioridadDAL.GetAllAsync();
        }
    }
}
