using GestordeTareas.EN;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GestordeTareas.DAL
{
    public class ImagenesPruebaDAL
    {
        //private readonly ContextoBD _context;

        public static async Task<int> CreateAsync(ImagenesPrueba imagenesTarea)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                dbContext.Add(imagenesTarea);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> UpdateAsync(ImagenesPrueba imagenesTarea)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var adImageDB = await dbContext.ImagenesTarea.FirstOrDefaultAsync(s => s.Id == imagenesTarea.Id);
                if (adImageDB != null)
                {
                    adImageDB.IdTarea = imagenesTarea.IdTarea;
                    adImageDB.Imagen = imagenesTarea.Imagen;
                    dbContext.Update(adImageDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<int> DeleteAsync(ImagenesPrueba imagenesTarea)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var adImageDB = await dbContext.ImagenesTarea.FirstOrDefaultAsync(s => s.Id == imagenesTarea.Id);
                if (adImageDB != null)
                {
                    dbContext.ImagenesTarea.Remove(adImageDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<ImagenesPrueba> GetByIdAsync(ImagenesPrueba imagenesTarea)
        {
            var adImageDB = new ImagenesPrueba();
            using (var dbContext = new ContextoBD())
            {
                adImageDB = await dbContext.ImagenesTarea.FirstOrDefaultAsync(s => s.Id == imagenesTarea.Id);
            }
            return adImageDB!;
        }

        public static async Task<List<ImagenesPrueba>> GetAllAsync()
        {
            var images = new List<ImagenesPrueba>();
            using (var dbContext = new ContextoBD())
            {
                images = await dbContext.ImagenesTarea.ToListAsync();
            }
            return images;
        }
        internal static IQueryable<ImagenesPrueba> QuerySelect(IQueryable<ImagenesPrueba> query, ImagenesPrueba adImage)
        {
            if (adImage.Id > 0)
                query = query.Where(s => s.Id == adImage.Id);

            if (adImage.IdTarea > 0)
                query = query.Where(s => s.IdTarea == adImage.IdTarea);

            if (!string.IsNullOrWhiteSpace(adImage.Imagen))
                query = query.Where(s => s.Imagen.Contains(adImage.Imagen));

            query = query.OrderByDescending(s => s.Id).AsQueryable();

            if (adImage.Top_Aux > 0)
                query = query.Take(adImage.Top_Aux).AsQueryable();
            return query;
        }

        public static async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesTarea)
        {
            var images = new List<ImagenesPrueba>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.ImagenesTarea.AsQueryable();
                select = QuerySelect(select, imagenesTarea);
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesTarea)
        {
            var images = new List<ImagenesPrueba>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.ImagenesTarea.AsQueryable();
                select = QuerySelect(select, imagenesTarea).Include(s => s.IdTareaFinalizada).AsQueryable();
                images = await select.ToListAsync();
            }
            return images;
        }
    }
}