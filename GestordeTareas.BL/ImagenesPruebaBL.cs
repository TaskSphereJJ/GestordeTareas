using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class ImagenesPruebaBL
    {

        public async Task<int> CreateAsync(ImagenesPrueba imagenesPruebas)
        {
            return await ImagenesPruebaDAL.CreateAsync(imagenesPruebas);
        }

        public async Task<int> UpdateAsync(ImagenesPrueba imagenesPruebas)
        {
            return await ImagenesPruebaDAL.UpdateAsync(imagenesPruebas);
        }

        public async Task<int> DeleteAsync(ImagenesPrueba imagenesPruebas)
        {
            return await ImagenesPruebaDAL.DeleteAsync(imagenesPruebas);
        }

        public async Task<ImagenesPrueba> GetByIdAsync(ImagenesPrueba imagenesPruebas)
        {
            return await ImagenesPruebaDAL.GetByIdAsync(imagenesPruebas);

        }


        public async Task<List<ImagenesPrueba>> GetAllAsync()
        {
            return await ImagenesPruebaDAL.GetAllAsync();
        }


        public async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesPruebas)
        {
            return await ImagenesPruebaDAL.SearchAsync(imagenesPruebas);
        }

        public async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesPruebas)
        {
            return await ImagenesPruebaDAL.SearchIncludeAdAsync(imagenesPruebas);

        }
    }
}
