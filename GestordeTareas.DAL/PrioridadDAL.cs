using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class PrioridadDAL
    {
        //--------------------------------METODO CREAR PRIORIDAD--------------------------
        public static async Task<int> CreateAsync(Prioridad prioridad)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                contextoBD.Prioridad.Add(prioridad);
                result = await contextoBD.SaveChangesAsync();
            }
            return result;
        }

        //--------------------------------METODO MODIFICAR PRIORIDAD--------------------------
        public static async Task<int> UpdateAsync(Prioridad prioridad)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                var prioridadBD = await contextoBD.Prioridad.FirstOrDefaultAsync(p => p.Id == prioridad.Id);
                if (prioridadBD != null)
                {
                    prioridadBD.Nombre = prioridad.Nombre;
                    contextoBD.Update(prioridadBD);
                    result = await contextoBD.SaveChangesAsync();
                }
            }
            return result;
        }

        //--------------------------------METODO ELIMINAR PRIORIDAD--------------------------
        public static async Task<int> DeleteAsync(Prioridad prioridad)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                var prioridadBD = await contextoBD.Prioridad.FirstOrDefaultAsync(p => p.Id == prioridad.Id);
                if (prioridadBD != null)
                {
                    contextoBD.Prioridad.Remove(prioridadBD);
                    result = await contextoBD.SaveChangesAsync();
                }
            }
            return result;
        }

        //--------------------------METODO BUSCAR POR ID--------------------------------------------
        public static async Task<Prioridad> GetByIdAsync(Prioridad prioridad)
        {
            using (var contextoBD = new ContextoBD())
            {
                return await contextoBD.Prioridad.FirstOrDefaultAsync(c => c.Id == prioridad.Id);
            }
        }

        //--------------------------------METODO OBTENER TODAS LAS PRIORIDADES--------------------------
        public static async Task<List<Prioridad>> GetAllAsync()
        {
            using (var contextoBD = new ContextoBD())
            {
                return await contextoBD.Prioridad.ToListAsync();
            }
        }
    }
}
