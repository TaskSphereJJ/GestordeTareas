using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class ImagenesPruebasBL
    {
        public async Task<int> CreateAsync(GestordeTaras.EN.ImagenesPruebas imagenesPruebas)
        {
            return await ImagenTareaDAL.CreateAsync(imagenesPruebas);
        }
        public async Task<int> UpdateAsync(GestordeTaras.EN.ImagenesPruebas imagenesPruebas)
        {
            return await ImagenTareaDAL.UpdateAsync(imagenesPruebas);
        }
        public async Task<int> DeleteAsync(GestordeTaras.EN.ImagenesPruebas imagenesPruebas)
        {
            return await ImagenTareaDAL.DeleteAsync(imagenesPruebas);
        }
        public async Task<GestordeTaras.EN.ImagenesPruebas> GetById(GestordeTaras.EN.ImagenesPruebas imagenesPruebas)
        {
            return await ImagenTareaDAL.GetByIdAsync(imagenesPruebas);
        }
        public async Task<List<GestordeTaras.EN.ImagenesPruebas>> GetAllAsync()
        {
            return await ImagenTareaDAL.GetAllAsync();
        }
    }
}
