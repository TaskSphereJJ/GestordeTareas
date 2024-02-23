using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class IniciarSesionDAL
    {
        private readonly ContextoBD _context;

        // Constructor que recibe una instancia de ContextoBD para la gestión de la base de datos.
        public IniciarSesionDAL(ContextoBD context)
        {
            _context = context;
        }

        // Método para agregar un objeto IniciarSesionEN a la base de datos de forma asincrónica.
        public async Task AgregarInicioSesionAsync(IniciarSesionEN inicioSesion)
        {
            _context.IniciarSesion.Add(inicioSesion);
            await _context.SaveChangesAsync();
        }

        // Método para obtener un objeto IniciarSesionEN por su Id de forma asincrónica.
        public async Task<IniciarSesionEN> ObtenerInicioSesionPorIdAsync(int id)
        {
            return await _context.IniciarSesion.FirstOrDefaultAsync(i => i.Id == id);
        }

        // Método para crear un nuevo objeto IniciarSesionEN en la base de datos de forma asincrónica.
        public async Task<int> CreateAsync(IniciarSesionEN iniciarSesion)
        {
            _context.Add(iniciarSesion);
            return await _context.SaveChangesAsync();
        }

        // Método para actualizar un objeto IniciarSesionEN en la base de datos de forma asincrónica.
        public async Task<int> UpdateAsync(IniciarSesionEN iniciarSesion)
        {
            // Busca el objeto existente en la base de datos por su Id.
            var inicioSesionDb = await _context.IniciarSesion.FirstOrDefaultAsync(i => i.Id == iniciarSesion.Id);

            // Si el objeto existe, actualiza sus propiedades y guarda los cambios.
            if (inicioSesionDb != null)
            {
                inicioSesionDb.NombreUsuario = iniciarSesion.NombreUsuario;
                inicioSesionDb.Pass = iniciarSesion.Pass;
                inicioSesionDb.IdUsuario = iniciarSesion.IdUsuario;

                return await _context.SaveChangesAsync();
            }

            return 0; // Otra opción sería lanzar una excepción indicando que no se encontró el registro.
        }

        // Método para eliminar un objeto IniciarSesionEN de la base de datos por su Id de forma asincrónica.
        public async Task DeleteAsync(int id)
        {
            // Busca el objeto existente en la base de datos por su Id.
            var inicioSesionDb = await _context.IniciarSesion.FirstOrDefaultAsync(i => i.Id == id);

            // Si el objeto existe, elimínalo de la base de datos y guarda los cambios.
            if (inicioSesionDb != null)
            {
                _context.IniciarSesion.Remove(inicioSesionDb);
                await _context.SaveChangesAsync();
            }
        }
    }

}
