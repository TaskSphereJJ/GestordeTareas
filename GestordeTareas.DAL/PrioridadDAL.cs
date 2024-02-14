using GestordeTaras.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class PrioridadDAL
    {
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
    }
}
