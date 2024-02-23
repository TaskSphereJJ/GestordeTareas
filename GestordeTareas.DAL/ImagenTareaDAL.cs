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
        public static async Task<int> CreateAsync(ImagenTarea imagenTarea)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                bdContexto.Add(imagenTarea);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> UpdateAsync(ImagenTarea imagenTarea)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var imagenTareaDB = await bdContexto.ImagenTarea.FirstOrDefaultAsync(i => i.Id == imagenTarea.Id);
                if (imagenTareaDB != null)
                {
                    imagenTareaDB.IdTarea = imagenTarea.IdTarea;
                    imagenTareaDB.RutaImagen = imagenTarea.RutaImagen;
                    bdContexto.Update(imagenTareaDB);
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<ImagenTarea> GetByIdAsync(ImagenTarea imagenTarea)
        {
            var imagenTareaDB = new ImagenTarea();
            using (var bdContexto = new ContextoBD())
            {
                imagenTareaDB = await bdContexto.ImagenTarea.FirstOrDefaultAsync(s => s.Id == imagenTarea.Id);
            }
            return imagenTareaDB;
        }

        public static async Task<List<ImagenTarea>> GetAllAsync()
        {
            var images = new List<ImagenTarea>();
            using (var bdContexto = new ContextoBD())
            {
                images = await bdContexto.ImagenTarea.ToListAsync();
            }
            return images;
        }

        internal static IQueryable<ImagenTarea> QuerySelect(IQueryable<ImagenTarea> query, ImagenTarea imagenTarea)
        {
            if (imagenTarea.Id > 0)
                query = query.Where(s => s.Id == imagenTarea.Id);
            if (imagenTarea.IdTarea > 0)
                query = query.Where(s => s.IdTarea == imagenTarea.IdTarea);
            if (!string.IsNullOrWhiteSpace(imagenTarea.RutaImagen))
                query = query.Where(s => s.RutaImagen.Contains(imagenTarea.RutaImagen));

            query = query.OrderByDescending(s => s.Id).AsQueryable();

            if (imagenTarea.Top_Aux > 0)
                query = query.Take(imagenTarea.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<ImagenTarea>> SearchAsync(ImagenTarea imagenTarea)
        {
            var images = new List<ImagenTarea>();
            using (var bdContexto = new ContextoBD())
            {
                var select = bdContexto.ImagenTarea.AsQueryable();
                select = QuerySelect(select, imagenTarea);
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<List<ImagenTarea>> SearchIncludeAdAsync(ImagenTarea imagenTarea)
        {
            var images = new List<ImagenTarea>();
            using (var bdContexto = new ContextoBD())
            {
                var select = bdContexto.ImagenTarea.AsQueryable();
                select = QuerySelect(select, imagenTarea).Include(i => i.Tarea).AsQueryable();
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<int> DeleteAsync(ImagenTarea imagenTarea)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var imagenTareaDB = await bdContexto.ImagenTarea.FirstOrDefaultAsync(i => i.Id == imagenTarea.Id);
                if (imagenTareaDB != null)
                {
                    bdContexto.ImagenTarea.Remove(imagenTareaDB);
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }

    }
}
