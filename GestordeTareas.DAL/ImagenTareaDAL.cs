using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GestordeTareas.DAL
{
    public class ImagenTareaDAL
    {
        public static async Task<int> CreateAsync(ImagenesPrueba imagenesPruebas)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                bdContexto.Add(imagenesPruebas);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> UpdateAsync(ImagenesPrueba imagenesPruebas)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD())
            {
                var imagenesPruebasDb = await dbContexto.ImagenesPrueba.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);

                if (imagenesPruebasDb != null)
                {
                    imagenesPruebasDb.Imagen = imagenesPruebas.Imagen;
                    imagenesPruebasDb.IdTareaFinalizada = imagenesPruebas.IdTareaFinalizada;


                    dbContexto.Update(imagenesPruebas);
                    result = await dbContexto.SaveChangesAsync();
                }
                return result;
            }

        }


        public static async Task<ImagenesPrueba> GetByIdAsync(ImagenesPrueba imagenesPruebas)
        {
            var imagenesPruebasDB = new ImagenesPrueba();
            using (var bdContexto = new ContextoBD())
            {
                imagenesPruebasDB = await bdContexto.ImagenesPrueba.FirstOrDefaultAsync(s => s.Id == imagenesPruebas.Id);
            }
            return imagenesPruebasDB;
        }

        public static async Task<List<ImagenesPrueba>> GetAllAsync()
        {
            var images = new List<ImagenesPrueba>();
            using (var bdContexto = new ContextoBD())
            {
                images = await bdContexto.ImagenesPrueba.ToListAsync();
            }
            return images;
        }

        internal static IQueryable<ImagenesPrueba> QuerySelect(IQueryable<ImagenesPrueba> query, ImagenesPrueba imagenesPruebas)
        {
            if (imagenesPruebas.Id > 0)
                query = query.Where(s => s.Id == imagenesPruebas.Id);
            if (imagenesPruebas.Id > 0)
                query = query.Where(s => s.Id == imagenesPruebas.Id);
            if (!string.IsNullOrWhiteSpace(imagenesPruebas.Imagen))
                query = query.Where(s => s.Imagen.Contains(imagenesPruebas.Imagen));

            query = query.OrderByDescending(s => s.Id).AsQueryable();

            //if (imagenTarea.Top_Aux > 0)
            //    query = query.Take(imagenTarea.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesPruebas)
        {
            var images = new List<ImagenesPrueba>();
            using (var bdContexto = new ContextoBD())
            {
                var select = bdContexto.ImagenesPrueba.AsQueryable();
                select = QuerySelect(select, imagenesPruebas);
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesPruebas)
        {
            var images = new List<ImagenesPrueba>();
            using (var bdContexto = new ContextoBD())
            {
                var select = bdContexto.ImagenesPrueba.AsQueryable();
                select = QuerySelect(select, imagenesPruebas).Include(i => i.IdTareaFinalizada).AsQueryable();
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<int> DeleteAsync(ImagenesPrueba imagenesPruebas)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var imagenesPruebasDB = await bdContexto.ImagenesPrueba.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);
                if (imagenesPruebasDB != null)
                {
                    bdContexto.ImagenesPrueba.Remove(imagenesPruebasDB);
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }

    }
}
