using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class TareaDAL
    {
        //--------------------------------METODO CREAR Tarea.--------------------------
        public static async Task<int> CreateAsync(Tarea tarea)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                contextoBD.Add(tarea);
                result = await contextoBD.SaveChangesAsync();
            }
            return result;
        }
        //--------------------------------METODO MODIFICAR TArea.--------------------------
        public static async Task<int> UpdateAsync(Tarea tarea)
        {
            int resul = 0;
            using (var contextoBD = new ContextoBD())
            {
                var tareaBD = await contextoBD.Tarea.FirstOrDefaultAsync(t => t.Id == tarea.Id);
                if (tareaBD != null)
                {
                    tareaBD.Nombre = tarea.Nombre;
                    contextoBD.Update(tareaBD);
                    resul = await contextoBD.SaveChangesAsync();
                }
                return resul;
            }
        }
        //...............--------------METODO ELIMINAR---------------------------
        public static async Task<int> DeleteAsync(Tarea tarea)
        {
            int result = 0;
            using (var contextoBD = new ContextoBD())
            {
                var tareaBD = await contextoBD.Tarea.FirstOrDefaultAsync(t => t.Id == tarea.Id);
                if (tarea != null)
                {
                    contextoBD.Tarea.Remove(tareaBD);
                    result = await contextoBD.SaveChangesAsync();
                }
                return result;
            }
        }

        //--------------------------METODO BUSCAR POR ID--------------------------------------------
        public static async Task<Tarea> GetByIdAsync(Tarea tarea)
        {
            var tareaBD = new Tarea();
            using (var bdContexto = new ContextoBD())
            {
                var priordadB = await bdContexto.Tarea.FirstOrDefaultAsync(c => c.Id == tarea.Id); //busco el id
            }
            return tareaBD;
        }
        //--------------------------------METODO obtener todas las PRIORIDADES.--------------------------
        public static async Task<List<Tarea>> GetAllAsync()
        {
            var tareas = new List<Tarea>(); //una variable de lo que llevara una lista de prioridades
            using (var bdContexto = new ContextoBD()) //creo el acceso a la BD
            {
                tareas = await bdContexto.Tarea.ToListAsync(); //le digo que categories contenga la lista de categorias, osea lo de l BD
            }
            return tareas;
        }
    }
}
