using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD:GestordeTareas.BL/ImagenesPruebaBL.cs
=======
using static System.Net.Mime.MediaTypeNames;
>>>>>>> dff9044fdaea6a59a1e888c274b2ff2395000048:GestordeTareas.BL/ImagenesPruebasBL.cs

namespace GestordeTareas.BL
{
    public class ImagenesPruebaBL
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
