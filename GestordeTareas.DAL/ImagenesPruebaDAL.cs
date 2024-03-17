using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{

    public class ImagenesPruebaDAL
    {
        public static async Task<int> CreateAsync(ImagenesPrueba imagenesPruebas)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD())
            {
                dbContexto.ImagenesPrueba.Add(imagenesPruebas);
                result = await dbContexto.SaveChangesAsync();
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


        public static async Task<int> DeleteAsync(ImagenesPrueba imagenesPruebas)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD())
            {
                var imagenesPruebasDb = await dbContexto.ImagenesPrueba.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);
                if (imagenesPruebasDb != null)
                {
                    dbContexto.ImagenesPrueba.Remove(imagenesPruebasDb);
                    result = await dbContexto.SaveChangesAsync();
                }
            }
            return result;
        }


        public static async Task<ImagenesPrueba> GetByIdAsync(ImagenesPrueba imagenesPruebas)
        {
            var imagenesPruebasDb = new ImagenesPrueba();
            using (var dbContext = new ContextoBD())
            {
                imagenesPruebasDb = await dbContext.ImagenesPrueba.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);
            }
            return imagenesPruebasDb!;
        }

        public static async Task<List<ImagenesPrueba>> GetAllAsync()
        {
            var _imagenesPruebas = new List<ImagenesPrueba>();
            using (var dbContexto = new ContextoBD())
            {
                _imagenesPruebas = await dbContexto.ImagenesPrueba.ToListAsync();
            }
            return _imagenesPruebas;
        }


        internal static IQueryable<ImagenesPrueba> QuerySelect(IQueryable<ImagenesPrueba> query, ImagenesPrueba imagenesPruebas)
        {
            if (imagenesPruebas.Id > 0)
                query = query.Where(s => s.Id == imagenesPruebas.Id);

            if (!string.IsNullOrWhiteSpace(imagenesPruebas.Imagen))
                query = query.Where(s => s.Imagen.Contains(imagenesPruebas.Imagen));

            if (imagenesPruebas.IdTareaFinalizada > 0)
                query = query.Where(s => s.IdTareaFinalizada == imagenesPruebas.IdTareaFinalizada);


            query = query.OrderByDescending(s => s.Id).AsQueryable();

            if (imagenesPruebas.Top_Aux > 0)
                query = query.Take(imagenesPruebas.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesPruebas)
        {
            var images = new List<ImagenesPrueba>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.ImagenesPrueba.AsQueryable();
                select = QuerySelect(select, imagenesPruebas);
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesPruebas)
        {
            var imagen = new List<ImagenesPrueba>();
            using (var dbContexto = new ContextoBD())
            {
                var select = dbContexto.ImagenesPrueba.AsQueryable();
                select = QuerySelect(select, imagenesPruebas).Include(i => i.TareaFinalizada).AsQueryable();
                imagen = await select.ToListAsync();
            }
            return imagen;
        }
    }
}
