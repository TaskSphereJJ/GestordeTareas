using System.Collections.Generic;
using GestordeTareas.DAL;
using GestordeTaras.EN;
using GestordeTareas.EN;
using static System.Net.Mime.MediaTypeNames;

namespace GestordeTareas.BL
{
    public class ImagenesPruebaBL
    {
        public async Task<int> CreateAsync(ImagenesPrueba imagenesTarea)
        {
            return await ImagenesPruebaDAL.CreateAsync(imagenesTarea);
        }

        public async Task<int> UpdateAsync(ImagenesPrueba imagenesTarea)
        {
            return await ImagenesPruebaDAL.UpdateAsync(imagenesTarea);
        }

        public async Task<int> DeleteAsync(ImagenesPrueba imagenesTarea)
        {
            return await ImagenesPruebaDAL.DeleteAsync(imagenesTarea);
        }

        public async Task<ImagenesPrueba> GetByIdAsync(ImagenesPrueba imagenesTarea)
        {
            return await ImagenesPruebaDAL.GetByIdAsync(imagenesTarea);
        }

        public async Task<List<ImagenesPrueba>> GetAllAsync()
        {
            return await ImagenesPruebaDAL.GetAllAsync();
        }

        public async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesTarea)
        {
            return await ImagenesPruebaDAL.SearchAsync(imagenesTarea);
        }

        public async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesTarea)
        {
            return await ImagenesPruebaDAL.SearchIncludeAdAsync(imagenesTarea);
        }
    }

}
