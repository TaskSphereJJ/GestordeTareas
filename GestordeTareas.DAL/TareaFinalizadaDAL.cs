using GestordeTaras.EN;
using GestordeTareas.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class TareaFinalizadaDAL
    {
        private readonly DbContext _context;

        public TareaFinalizadaDAL(DbContext context)
        {
            _context = context;
        }

        public async Task<int> AgregarTareaFinalizadaAsync(TareaFinalizada tareaFinalizada)
        {
            _context.Add(tareaFinalizada);
            await _context.SaveChangesAsync();
            return tareaFinalizada.Id;
        }

        public async Task AgregarImagenesTareaAsync(List<ImagenesPrueba> imagenes)
        {
            if (imagenes == null || imagenes.Count == 0)
                throw new ArgumentException("No hay imágenes para agregar.");

            _context.AddRange(imagenes);
            await _context.SaveChangesAsync();
        }
    }

}
