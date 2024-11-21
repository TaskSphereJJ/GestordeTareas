using GestordeTaras.EN;
using GestordeTareas.DAL;
using GestordeTareas.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class TareaFinalizadaBL
    {
        private readonly TareaFinalizadaDAL _tareaFinalizadaDAL;

        public TareaFinalizadaBL(DbContext context)
        {
            _tareaFinalizadaDAL = new TareaFinalizadaDAL(context);
        }

        public async Task<int> CrearTareaFinalizadaConImagenesAsync(TareaFinalizada tareaFinalizada, List<string> rutasImagenes)
        {
            if (tareaFinalizada == null)
                throw new ArgumentNullException(nameof(tareaFinalizada), "La tarea finalizada no puede ser nula.");

            if (rutasImagenes == null || rutasImagenes.Count == 0)
                throw new ArgumentException("Debe proporcionar al menos una imagen.");

            // Crear tarea finalizada en la base de datos
            int idTareaFinalizada = await _tareaFinalizadaDAL.AgregarTareaFinalizadaAsync(tareaFinalizada);

            // Asignar las imágenes a la tarea finalizada
            var imagenes = rutasImagenes.Select(ruta => new ImagenesPrueba
            {
                Imagen = ruta,
                IdTareaFinalizada = idTareaFinalizada
            }).ToList();

            // Guardar imágenes en la base de datos
            await _tareaFinalizadaDAL.AgregarImagenesTareaAsync(imagenes);

            return idTareaFinalizada;
        }
    }

}
