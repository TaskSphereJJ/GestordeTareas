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
        public async Task<int> CreateAsync(EstadoTareaEN estadoTarea)
        {
            return await EstadoTareaDAL.CreateAsync(estadoTarea);
        }
        public async Task<int> UpdateAsync(EstadoTareaEN estadoTarea)
        {
            return await EstadoTareaDAL.UpdateAsync(estadoTarea);
        }
        public async Task<int> DeleteAsync(EstadoTareaEN estadoTarea)
        {
            return await EstadoTareaDAL.DeleteAsync(estadoTarea);
        }
        public async Task<EstadoTareaEN> GetById(EstadoTareaEN estadoTarea)
        {
            return await EstadoTareaDAL.GetByIdAsync(estadoTarea);
        }
        public async Task<List<EstadoTareaEN>> GetAllAsync()
        {
            return await EstadoTareaDAL.GetAllAsync();
        }
    }
}
