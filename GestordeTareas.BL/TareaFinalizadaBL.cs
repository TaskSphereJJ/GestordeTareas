//using GestordeTaras.EN;
//using GestordeTareas.DAL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GestordeTareas.BL
//{
//    public class TareaFinalizadaBL
//    {
//        // Crear una tarea finalizada
//        public async Task<int> CreateAsync(TareaFinalizada tareaFinalizada)
//        {
//            if (string.IsNullOrWhiteSpace(tareaFinalizada.Comentarios))
//            {
//                throw new ArgumentException("Los comentarios son obligatorios.");
//            }

//            if (tareaFinalizada.FechaFinalizacion > DateTime.Now)
//            {
//                throw new ArgumentException("La fecha de finalización no puede ser una fecha futura.");
//            }

//            return await TareaFinalizadaDAL.CreateAsync(tareaFinalizada);
//        }

//        // Actualizar una tarea finalizada
//        public async Task<int> UpdateAsync(TareaFinalizada tareaFinalizada)
//        {
//            if (tareaFinalizada.Id <= 0)
//            {
//                throw new ArgumentException("El ID de la tarea debe ser mayor que cero.");
//            }

//            if (string.IsNullOrWhiteSpace(tareaFinalizada.Comentarios))
//            {
//                throw new ArgumentException("Los comentarios son obligatorios.");
//            }

//            if (tareaFinalizada.FechaFinalizacion > DateTime.Now)
//            {
//                throw new ArgumentException("La fecha de finalización no puede ser una fecha futura.");
//            }

//            return await TareaFinalizadaDAL.UpdateAsync(tareaFinalizada);
//        }

//        // Eliminar una tarea finalizada
//        public async Task<int> DeleteAsync(TareaFinalizada tareaFinalizada)
//        {
//            if (tareaFinalizada.Id <= 0)
//            {
//                throw new ArgumentException("El ID de la tarea debe ser mayor que cero.");
//            }

//            return await TareaFinalizadaDAL.DeleteAsync(tareaFinalizada);
//        }

//        // Obtener una tarea finalizada por ID
//        public async Task<TareaFinalizada> GetByIdAsync(int id)
//        {
//            if (id <= 0)
//            {
//                throw new ArgumentException("El ID debe ser mayor que cero.");
//            }

//            var tarea = new TareaFinalizada { Id = id };
//            return await TareaFinalizadaDAL.GetByIdAsync(tarea);
//        }

//        // Obtener todas las tareas finalizadas
//        public async Task<List<TareaFinalizada>> GetAllAsync()
//        {
//            return await TareaFinalizadaDAL.GetAllAsync();
//        }

//        // Método que crea una tarea finalizada con sus imágenes asociadas
//        public async Task<int> CreateWithImagesAsync(TareaFinalizada tareaFinalizada, List<ImagenesPrueba> imagenesPruebas)
//        {
//            using (var dbContexto = new ContextoBD())
//            {
//                using (var transaction = await dbContexto.Database.BeginTransactionAsync())
//                {
//                    try
//                    {
//                        // Crear la tarea finalizada
//                        await TareaFinalizadaDAL.CreateAsync(tareaFinalizada);

//                        // Agregar las imágenes
//                        foreach (var imagen in imagenesPruebas)
//                        {
//                            imagen.IdTareaFinalizada = tareaFinalizada.Id;
//                            await ImagenesPruebaDAL.CreateAsync(imagen);
//                        }

//                        // Commit de la transacción si todo es exitoso
//                        await transaction.CommitAsync();
//                        return 1;
//                    }
//                    catch (Exception)
//                    {
//                        // Rollback en caso de error
//                        await transaction.RollbackAsync();
//                        throw;
//                    }
//                }
//            }
//        }
//    }
//}
