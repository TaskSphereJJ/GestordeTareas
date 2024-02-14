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
    }
}
