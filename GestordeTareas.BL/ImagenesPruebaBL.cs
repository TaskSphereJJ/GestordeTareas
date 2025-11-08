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

        public async Task<int> CreateAsync(ImagenesPrueba imagenesPrueba)
        {
            return await ImagenesPruebaDAL.CreateAsync(imagenesPrueba);
        }

        public async Task<int> UpdateAsync(ImagenesPrueba imagenesPrueba)
        {
            return await ImagenesPruebaDAL.UpdateAsync(imagenesPrueba);
        }

        public async Task<int> DeleteAsync(ImagenesPrueba imagenesPrueba)
        {
            return await ImagenesPruebaDAL.DeleteAsync(imagenesPrueba);
        }

        public async Task<ImagenesPrueba> GetByIdAsync(ImagenesPrueba imagenesPrueba)
        {
            return await ImagenesPruebaDAL.GetByIdAsync(imagenesPrueba);

        }


        public async Task<List<ImagenesPrueba>> GetAllAsync()
        {
            return await ImagenesPruebaDAL.GetAllAsync();
        }


        public async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesPrueba)
        {
            return await ImagenesPruebaDAL.SearchAsync(imagenesPrueba);
        }

        public async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesPrueba)
        {
            return await ImagenesPruebaDAL.SearchIncludeAdAsync(imagenesPrueba);

        }
    }
}
