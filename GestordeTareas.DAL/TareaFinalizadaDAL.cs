using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class TareaFinalizadaDAL
    {
        //--------------------------------METODO CREAR TAREA FINALIZADA.--------------------------
        public static async Task<int> CreateAsync(TareaFinalizada tareaFinalizada)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD()) //el comando using hace un proceso de ejecucion
            {
                dbContexto.TareaFinalizada.Add(tareaFinalizada); //agrego una nueva tarea finalizada
                result = await dbContexto.SaveChangesAsync();//se guarda a la base de datos
            }
            return result;
        }
        //--------------------------------METODO MODIFICAR TAREA FINALZADA.--------------------------
        public static async Task<int> UpdateAsync(TareaFinalizada tareaFinalizada)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())//hago una instancia de la base de datos
            {
                //expresion landam
                var tareaFinalizadaBD = await bdContexto.TareaFinalizada.FirstOrDefaultAsync(t => t.Id == tareaFinalizada.Id); //lo busco 
                if (tareaFinalizadaBD != null)//verifico que no este nulo
                {
                    tareaFinalizadaBD.FechaFinalizacion = tareaFinalizada.FechaFinalizacion; //actualizo las propiedades
                    tareaFinalizadaBD.Comentarios = tareaFinalizada.Comentarios;

                    bdContexto.Update(tareaFinalizadaBD); //se guarda en memoria
                    result = await bdContexto.SaveChangesAsync(); //guardo en la base de datos con SaveChangesAsync
                }
            }
            return result;
        }
        //--------------------------------METODO Eliminar TAREA FINALZADA.--------------------------
        public static async Task<int> DeleteAsync(TareaFinalizada tareaFinalizada)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD()) //istancio la coneccion
            {
                var tareaFinalizadaBD = await bdContexto.TareaFinalizada.FirstOrDefaultAsync(t => t.Id == tareaFinalizada.Id); //busco el id
                if (tareaFinalizadaBD != null)//verifico que no este nulo
                {
                    bdContexto.TareaFinalizada.Remove(tareaFinalizadaBD);//elimino anivel de memoria la Tarea Finalizada
                    result = await bdContexto.SaveChangesAsync();//le digo a la BD que se elimine y se guarde
                }
            }
            return result;
        }
        //--------------------------------METODO obtenerporID TAREA FINALZADA.--------------------------
        public static async Task<TareaFinalizada> GetByIdAsync(TareaFinalizada tareaFinalizada)
        {
            var tareaFinalBD = new TareaFinalizada();
            using (var bdContexto = new ContextoBD())
            {
                tareaFinalBD = await bdContexto.TareaFinalizada.FirstOrDefaultAsync(t => t.Id == tareaFinalizada.Id); //busco el id
            }
            return tareaFinalBD;
        }

        //--------------------------------METODO obtener todas las CATEGORIAS.--------------------------
        public static async Task<List<TareaFinalizada>> GetAllAsync()
        {
            var tareasFinalizadas = new List<TareaFinalizada>(); //una variable de lo que llevara una lista de Tarea Finalizadas
            using (var bdContexto = new ContextoBD()) //creo el acceso a la BD
            {
                tareasFinalizadas = await bdContexto.TareaFinalizada.ToListAsync(); //le digo que categories contenga la lista de categorias, osea lo de l BD
            }
            return tareasFinalizadas;
        }
    }
}
