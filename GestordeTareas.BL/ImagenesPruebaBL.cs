//using GestordeTaras.EN;
//using GestordeTareas.DAL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GestordeTareas.BL
//{
//    public class ImagenesPruebaBL
//    {
//        // Crear una imagen de prueba
//        public async Task<int> CreateAsync(ImagenesPrueba imagenesPruebas)
//        {
//            if (string.IsNullOrEmpty(imagenesPruebas.Imagen))
//            {
//                throw new ArgumentException("La ruta de la imagen no puede estar vacía.");
//            }

//            return await ImagenesPruebaDAL.CreateAsync(imagenesPruebas);
//        }

//        // Actualizar una imagen de prueba
//        public async Task<int> UpdateAsync(ImagenesPrueba imagenesPruebas)
//        {
//            if (imagenesPruebas.Id <= 0)
//            {
//                throw new ArgumentException("El ID de la imagen debe ser mayor que cero.");
//            }

//            if (string.IsNullOrEmpty(imagenesPruebas.Imagen))
//            {
//                throw new ArgumentException("La ruta de la imagen no puede estar vacía.");
//            }

//            return await ImagenesPruebaDAL.UpdateAsync(imagenesPruebas);
//        }

//        // Eliminar una imagen de prueba
//        public async Task<int> DeleteAsync(ImagenesPrueba imagenesPruebas)
//        {
//            if (imagenesPruebas.Id <= 0)
//            {
//                throw new ArgumentException("El ID de la imagen debe ser mayor que cero.");
//            }

//            return await ImagenesPruebaDAL.DeleteAsync(imagenesPruebas);
//        }

//        public async Task<ImagenesPrueba> GetByIdAsync(int id)
//        {
//            if (id <= 0)
//            {
//                throw new ArgumentException("El ID debe ser mayor que cero.");
//            }

//            var imagen = new ImagenesPrueba { Id = id };
//            return await ImagenesPruebaDAL.GetByIdAsync(imagen);
//        }

//        // Obtener todas las imágenes de prueba
//        public async Task<List<ImagenesPrueba>> GetAllAsync()
//        {
//            return await ImagenesPruebaDAL.GetAllAsync();
//        }

//        public async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesPruebas)
//        {
//            return await ImagenesPruebaDAL.SearchAsync(imagenesPruebas);
//        }

//        public async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesPruebas)
//        {
//            return await ImagenesPruebaDAL.SearchIncludeAdAsync(imagenesPruebas);
//        }
//    }
//}
