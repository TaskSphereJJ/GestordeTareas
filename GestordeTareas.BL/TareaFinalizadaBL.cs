using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class TareaFinalizadaBL
    {
        public async Task<int> CreateAsync(TareaFinalizada tareaFinalizada)
        {
            return await TareaFinalizadaDAL.CreateAsync(tareaFinalizada);
        }
        public async Task<int> UpdateAsync(TareaFinalizada tareaFinalizada)
        {
            return await TareaFinalizadaDAL.UpdateAsync(tareaFinalizada);
        }
        public async Task<int> DeleteAsync(TareaFinalizada tareaFinalizada)
        {
            return await TareaFinalizadaDAL.DeleteAsync(tareaFinalizada);
        }
        public async Task<TareaFinalizada> GetById(TareaFinalizada tareaFinalizada)
        {
            return await TareaFinalizadaDAL.GetByIdAsync(tareaFinalizada);
        }
        public async Task<List<TareaFinalizada>> GetAllAsync()
        {
            return await TareaFinalizadaDAL.GetAllAsync();
        }
    }
}
