using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class EstadoTareaBL
    {
        public async Task<int> CreateAsync(EstadoTarea estadoTarea)
        {
            return await EstadoTareaDAL.CreateAsync(estadoTarea);
        }
        public async Task<int> UpdateAsync(EstadoTarea estadoTarea)
        {
            return await EstadoTareaDAL.UpdateAsync(estadoTarea);
        }
        public async Task<int> DeleteAsync(EstadoTarea estadoTarea)
        {
            return await EstadoTareaDAL.DeleteAsync(estadoTarea);
        }
        public async Task<EstadoTarea> GetById(EstadoTarea estadoTarea)
        {
            return await EstadoTareaDAL.GetByIdAsync(estadoTarea);
        }
        public async Task<List<EstadoTarea>> GetAllAsync()
        {
            return await EstadoTareaDAL.GetAllAsync();
        }
    }
}
