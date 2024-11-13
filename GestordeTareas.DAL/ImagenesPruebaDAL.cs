//using GestordeTaras.EN;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GestordeTareas.DAL
//{

//    public class ImagenesPruebaDAL
//    {
//        public static async Task<int> CreateAsync(ImagenesPrueba imagenesPrueba)
//        {
//            int result = 0;
//            using (var dbContexto = new ContextoBD())
//            {
//                dbContexto.ImagenesPrueba.Add(imagenesPrueba);
//                result = await dbContexto.SaveChangesAsync();
//            }
//            return result;
//        }

//        public static async Task<int> UpdateAsync(ImagenesPrueba imagenesPrueba)
//        {
//            int result = 0;
//            using (var dbContexto = new ContextoBD())
//            {
//                var imagenesPruebaDb = await dbContexto.ImagenesPrueba.FirstOrDefaultAsync(i => i.Id == imagenesPrueba.Id);

//                if (imagenesPruebaDb != null)
//                {
//                    imagenesPruebaDb.Imagen = imagenesPrueba.Imagen;
//                    imagenesPruebaDb.IdTareaFinalizada = imagenesPrueba.IdTareaFinalizada;


//                    dbContexto.Update(imagenesPrueba);
//                    result = await dbContexto.SaveChangesAsync();
//                }
//                return result;
//            }

//        }



//        public static async Task<int> DeleteAsync(ImagenesPrueba imagenesPrueba)

//        {
//            int result = 0;
//            using (var dbContexto = new ContextoBD())
//            {

//                var imagenesPruebaDb = await dbContexto.ImagenesPrueba.FirstOrDefaultAsync(i => i.Id == imagenesPrueba.Id);
//                if (imagenesPruebaDb != null)
//                {
//                    dbContexto.ImagenesPrueba.Remove(imagenesPruebaDb);

//                    result = await dbContexto.SaveChangesAsync();
//                }
//            }
//            return result;
//        }



//        public static async Task<ImagenesPrueba> GetByIdAsync(ImagenesPrueba imagenesPrueba)
//        {
//            var imagenesPruebaDb = new ImagenesPrueba();
//            using (var dbContext = new ContextoBD())
//            {
//                imagenesPruebaDb = await dbContext.ImagenesPrueba.FirstOrDefaultAsync(i => i.Id == imagenesPrueba.Id);

//            }
//            return imagenesPruebaDb!;
//        }

//        public static async Task<List<ImagenesPrueba>> GetAllAsync()
//        {

//            var _imagenesPrueba = new List<ImagenesPrueba>();
//            using (var dbContexto = new ContextoBD())
//            {
//                _imagenesPrueba = await dbContexto.ImagenesPrueba.ToListAsync();

//            }
//            return _imagenesPrueba;
//        }



//        internal static IQueryable<ImagenesPrueba> QuerySelect(IQueryable<ImagenesPrueba> query, ImagenesPrueba imagenesPrueba)
//        {
//            if (imagenesPrueba.Id > 0)
//                query = query.Where(s => s.Id == imagenesPrueba.Id);

//            if (!string.IsNullOrWhiteSpace(imagenesPrueba.Imagen))
//                query = query.Where(s => s.Imagen.Contains(imagenesPrueba.Imagen));

//            if (imagenesPrueba.IdTareaFinalizada > 0)
//                query = query.Where(s => s.IdTareaFinalizada == imagenesPrueba.IdTareaFinalizada);


//            query = query.OrderByDescending(s => s.Id).AsQueryable();

//            if (imagenesPrueba.Top_Aux > 0)
//                query = query.Take(imagenesPrueba.Top_Aux).AsQueryable();

//            return query;
//        }


//        public static async Task<List<ImagenesPrueba>> SearchAsync(ImagenesPrueba imagenesPrueba)

//        {
//            var images = new List<ImagenesPrueba>();
//            using (var dbContext = new ContextoBD())
//            {
//                var select = dbContext.ImagenesPrueba.AsQueryable();

//                select = QuerySelect(select, imagenesPrueba);

//                images = await select.ToListAsync();
//            }
//            return images;
//        }


//        public static async Task<List<ImagenesPrueba>> SearchIncludeAdAsync(ImagenesPrueba imagenesPrueba)

//        {
//            var imagen = new List<ImagenesPrueba>();
//            using (var dbContexto = new ContextoBD())
//            {
//                var select = dbContexto.ImagenesPrueba.AsQueryable();

//                select = QuerySelect(select, imagenesPrueba).Include(i => i.TareaFinalizada).AsQueryable();

//                imagen = await select.ToListAsync();
//            }
//            return imagen;
//        }
//    }
//}
