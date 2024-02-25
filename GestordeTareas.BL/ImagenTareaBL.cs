using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class ImagenTareaBL
    {
        public async Task<int> CreateAsync(ImagenTarea imagenTarea)
        {
            return await ImagenTareaDAL.CreateAsync(imagenTarea);
        }
        public async Task<int> UpdateAsync(ImagenTarea imagenTarea)
        {
            return await ImagenTareaDAL.UpdateAsync(imagenTarea);
        }
        public async Task<int> DeleteAsync(ImagenTarea imagenTarea)
        {
            return await ImagenTareaDAL.DeleteAsync(imagenTarea);
        }
        public async Task<ImagenTarea> GetById(ImagenTarea imagenTarea)
        {
            return await ImagenTareaDAL.GetByIdAsync(imagenTarea);
        }
        public async Task<List<ImagenTarea>> GetAllAsync()
        {
            return await ImagenTareaDAL.GetAllAsync();
        }
    }
}
