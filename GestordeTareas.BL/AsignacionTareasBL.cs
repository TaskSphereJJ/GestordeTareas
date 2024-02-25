using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class AsignacionTareasBL
    {
        public async Task<int> CreateAsync(AsignacionTareas asignacionTareas)
        {
            return await AsignacionTareasDAL.CreateAsync(asignacionTareas);
        }
        public async Task<int> UpdateAsync(AsignacionTareas asignacionTareas)
        {
            return await AsignacionTareasDAL.UpdateAsync(asignacionTareas);
        }
        public async Task<int> DeleteAsync(AsignacionTareas asignacionTareas)
        {
            return await AsignacionTareasDAL.DeleteAsync(asignacionTareas);
        }
        public async Task<AsignacionTareas> GetById(AsignacionTareas asignacionTareas)
        {
            return await AsignacionTareasDAL.GetByIdAsync(asignacionTareas);
        }
        public async Task<List<AsignacionTareas>> GetAllAsync()
        {
            return await AsignacionTareasDAL.GetAllAsync();

        }
    }
}
