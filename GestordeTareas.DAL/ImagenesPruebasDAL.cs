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
    public class ImagenesPruebasDAL
    {
        public static async Task<int> CreateAsync(ImagenesPruebas imagenesPruebas)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD())
            {
                dbContexto.ImagenesPruebas.Add(imagenesPruebas);
                result = await dbContexto.SaveChangesAsync();
            }
            return result; 
        }

        public static async Task<int> UpdateAsync(ImagenesPruebas imagenesPruebas)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD())
            {
                var imagenesPruebasDb = await dbContexto.ImagenesPruebas.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);

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


        public static async Task<int> DeleteAsync(ImagenesPruebas imagenesPruebas)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD())
            {
                var imagenesPruebasDb = await dbContexto.ImagenesPruebas.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);
                if (imagenesPruebasDb != null)
                {
                    dbContexto.ImagenesPruebas.Remove(imagenesPruebasDb);
                    result = await dbContexto.SaveChangesAsync();
                }
            }
            return result;
        }


        public static async Task<ImagenesPruebas> GetByIdAsync(ImagenesPruebas imagenesPruebas)
        {
            var imagenesPruebasDb = new ImagenesPruebas();
            using (var dbContext = new ContextoBD())
            {
                imagenesPruebasDb = await dbContext.ImagenesPruebas.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);
            }
            return imagenesPruebasDb!;
        }

        public static async Task<List<ImagenesPruebas>> GetAllAsync()
        {
            var _imagenesPruebas = new List<ImagenesPruebas>();
            using (var dbContexto = new ContextoBD())
            {
                _imagenesPruebas = await dbContexto.ImagenesPruebas.ToListAsync();
            }
            return _imagenesPruebas;
        }


        internal static IQueryable<ImagenesPruebas> QuerySelect(IQueryable<ImagenesPruebas> query, ImagenesPruebas imagenesPruebas)
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

        public static async Task<List<ImagenesPruebas>> SearchAsync(ImagenesPruebas imagenesPruebas)
        {
            var images = new List<ImagenesPruebas>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.ImagenesPruebas.AsQueryable();
                select = QuerySelect(select, imagenesPruebas);
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<List<ImagenesPruebas>> SearchIncludeAdAsync(ImagenesPruebas imagenesPruebas)
        {
            var imagen = new List<ImagenesPruebas>();
            using (var dbContexto = new ContextoBD())
            {
                var select = dbContexto.ImagenesPruebas.AsQueryable();
                select = QuerySelect(select, imagenesPruebas).Include(i => i.TareaFinalizada).AsQueryable();
                imagen = await select.ToListAsync();
            }
            return imagen;
        }
    }
}
