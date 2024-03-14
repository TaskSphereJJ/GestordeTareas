using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace GestordeTareas.BL
{
    public class ImagenesPruebasBL
    {

        public async Task<int> CreateAsync(ImagenesPruebas imagenesPruebas)
        {
            return await ImagenesPruebasDAL.CreateAsync(imagenesPruebas);
        }

        public async Task<int> UpdateAsync(ImagenesPruebas imagenesPruebas)
        {
            return await ImagenesPruebasDAL.UpdateAsync(imagenesPruebas);
        }

        public async Task<int> DeleteAsync(ImagenesPruebas imagenesPruebas)
        {
            return await ImagenesPruebasDAL.DeleteAsync(imagenesPruebas);
        }

        public async Task<ImagenesPruebas> GetByIdAsync(ImagenesPruebas imagenesPruebas)
        {
            return await ImagenesPruebasDAL.GetByIdAsync(imagenesPruebas);
        }


        public async Task<List<ImagenesPruebas>> GetAllAsync()
        {
            return await ImagenesPruebasDAL.GetAllAsync();
        }

        public async Task<List<ImagenesPruebas>> SearchAsync(ImagenesPruebas imagenesPruebas)
        {
            return await ImagenesPruebasDAL.SearchAsync(imagenesPruebas);
        }

        public async Task<List<ImagenesPruebas>> SearchIncludeAdAsync(ImagenesPruebas imagenesPruebas)
        {
            return await ImagenesPruebasDAL.SearchIncludeAdAsync(imagenesPruebas);
        }
    }
}
