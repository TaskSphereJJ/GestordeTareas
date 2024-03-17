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
        public static async Task<int> CreateAsync(ImagenesPruebas imagenesPruebas)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                bdContexto.Add(imagenesPruebas);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> UpdateAsync(ImagenesPruebas imagenesPruebas)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var imagenesPruebasDB = await bdContexto.ImagenesPruebas.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);
                if (imagenesPruebasDB != null)
                {
                    imagenesPruebasDB.Id = imagenesPruebas.Id;
                    imagenesPruebasDB.Imagen = imagenesPruebas.Imagen;
                    bdContexto.Update(imagenesPruebasDB);
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<ImagenesPruebas> GetByIdAsync(ImagenesPruebas imagenesPruebas)
        {
            var imagenesPruebasDB = new ImagenesPruebas();
            using (var bdContexto = new ContextoBD())
            {
                imagenesPruebasDB = await bdContexto.ImagenesPruebas.FirstOrDefaultAsync(s => s.Id == imagenesPruebas.Id);
            }
            return imagenesPruebasDB;
        }

        public static async Task<List<ImagenesPruebas>> GetAllAsync()
        {
            var images = new List<ImagenesPruebas>();
            using (var bdContexto = new ContextoBD())
            {
                images = await bdContexto.ImagenesPruebas.ToListAsync();
            }
            return images;
        }

        internal static IQueryable<ImagenesPruebas> QuerySelect(IQueryable<ImagenesPruebas> query, ImagenesPruebas imagenesPruebas)
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

        public static async Task<List<ImagenesPruebas>> SearchAsync(ImagenesPruebas imagenesPruebas)
        {
            var images = new List<ImagenesPruebas>();
            using (var bdContexto = new ContextoBD())
            {
                var select = bdContexto.ImagenesPruebas.AsQueryable();
                select = QuerySelect(select, imagenesPruebas);
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<List<ImagenesPruebas>> SearchIncludeAdAsync(ImagenesPruebas imagenesPruebas)
        {
            var images = new List<ImagenesPruebas>();
            using (var bdContexto = new ContextoBD())
            {
                var select = bdContexto.ImagenesPruebas.AsQueryable();
                select = QuerySelect(select, imagenesPruebas).Include(i => i.IdTareaFinalizada).AsQueryable();
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<int> DeleteAsync(ImagenesPruebas imagenesPruebas)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var imagenesPruebasDB = await bdContexto.ImagenesPruebas.FirstOrDefaultAsync(i => i.Id == imagenesPruebas.Id);
                if (imagenesPruebasDB != null)
                {
                    bdContexto.ImagenesPruebas.Remove(imagenesPruebasDB);
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }

    }
}
