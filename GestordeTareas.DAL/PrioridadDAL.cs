using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class PrioridadDAL
    {
        //--------------------------------METODO CREAR CARGO.--------------------------
        public static async Task<int> CreateAsync(Prioridad prioridad)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                contextoBD.Add(prioridad);
                result = await contextoBD.SaveChangesAsync();
            }
            return result;
        }
        //--------------------------------METODO MODIFICAR cargo.--------------------------
        public static async Task<int> UpdateAsync(Prioridad prioridad)
        {
            int resul = 0;
            using (var contextoBD = new ContextoBD())
            {
                var prioridadBD = await contextoBD.Prioridad.FirstOrDefaultAsync(p => p.Id == prioridad.Id);
                if (prioridadBD != null)
                {
                    prioridadBD.Nombre = prioridad.Nombre;
                    contextoBD.Update(prioridadBD);
                    resul = await contextoBD.SaveChangesAsync();
                }
                return resul;
            }
        }
        //...............--------------METODO ELIMINAR---------------------------
        public static async Task<int> DeleteAsyc(Prioridad prioridad)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                var prioridadBD = await contextoBD.Prioridad.FirstOrDefaultAsync(p => p.Id == prioridad.Id);
                if (prioridad != null)
                {
                    contextoBD.Prioridad.Remove(prioridadBD);
                    result = await contextoBD.SaveChangesAsync();
                }
                return result;
            }
        }

        //--------------------------METODO BUSCAR POR ID--------------------------------------------
        public static async Task<Prioridad> GetByIdAsync(Prioridad prioridad)
        {
            var prioridadBD = new Prioridad();
            using (var bdContexto = new ContextoBD())
            {
                 prioridadBD = await bdContexto.Prioridad.FirstOrDefaultAsync(c => c.Id == prioridad.Id); //busco el id
            }
            return prioridadBD;
        }
        //--------------------------------METODO obtener todas las PRIORIDADES.--------------------------
        public static async Task<List<Prioridad>> GetAllAsync()
        {
            var priodidades = new List<Prioridad>(); //una variable de lo que llevara una lista de prioridades
            using (var bdContexto = new ContextoBD()) //creo el acceso a la BD
            {
                priodidades = await bdContexto.Prioridad.ToListAsync(); //le digo que categories contenga la lista de categorias, osea lo de l BD
            }
            return priodidades;
        }

        // Método para eliminar prioridad de la base de datos de forma asincrónica.
        public static async Task<int> DeleteAsync(Prioridad prioridad)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                // Busca la prioridad existente por su ID.
                var prioridadBD = await contextoBD.Prioridad.FirstOrDefaultAsync(p => p.Id == prioridad.Id);
                if (prioridadBD != null)
                {
                    // Elimina la prioridad del DbSet correspondiente en el contexto.
                    contextoBD.Prioridad.Remove(prioridadBD);
                    // Guarda los cambios en la base de datos.
                    result = await contextoBD.SaveChangesAsync();
                }
            }
            // Retorna el resultado (número de filas afectadas en la base de datos).
            return result;
        }

    }
}
