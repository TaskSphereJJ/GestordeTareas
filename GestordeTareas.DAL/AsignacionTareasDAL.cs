using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class AsignacionTareasDAL
    {
        //--------------------------------METODO CREAR AsignacionTareas.--------------------------
        public static async Task<int> CreateAsync(AsignacionTareas asignacionTareas)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD()) //el comando using hace un proceso de ejecucion
            {
                dbContexto.AsignacionTareas.Add(asignacionTareas); //agrego un nuevo AsignacionTareas
                result = await dbContexto.SaveChangesAsync();//se guarda a la base de datos
            }
            return result;
        }

        //--------------------------------METODO MODIFICAR AsignacionTareas.--------------------------
        public static async Task<int> UpdateAsync(AsignacionTareas asignacionTareas)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var asignacionTareasBD = await bdContexto.AsignacionTareas.FirstOrDefaultAsync(a => a.Id == asignacionTareas.Id);
                if (asignacionTareasBD != null)
                {
                    asignacionTareasBD.FechaAsignada = asignacionTareas.FechaAsignada;
                    asignacionTareasBD.FechaFinalizacion = asignacionTareas.FechaFinalizacion;
                    asignacionTareasBD.IdUsuario = asignacionTareas.IdUsuario;

                    bdContexto.Update(asignacionTareasBD);
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }

        //--------------------------------METODO ELIMINAR Asignacion tarea.--------------------------
        public static async Task<int> DeleteAsync(AsignacionTareas asignacionTareas)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD()) //istancio la conexion
            {
                var asignacionTareasBD = await bdContexto.AsignacionTareas.FirstOrDefaultAsync(a => a.Id == asignacionTareas.Id); //busco el id
                if (asignacionTareasBD != null)//verifico que no este nulo
                {
                    bdContexto.AsignacionTareas.Remove(asignacionTareasBD);//elimino anivel de memoria asignacionTareas
                    result = await bdContexto.SaveChangesAsync();//le digo a la BD que se elimine y se guarde
                }
            }
            return result;
        }

        //--------------------------------METODO obtenerporID AsignacionTareas.--------------------------
        public static async Task<AsignacionTareas> GetByIdAsync(AsignacionTareas asignacionTareas)
        {
            var asignacionTareasBD = new AsignacionTareas();
            using (var bdContexto = new ContextoBD())
            {
                var cargob = await bdContexto.AsignacionTareas.FirstOrDefaultAsync(a => a.Id == asignacionTareas.Id); //busco el id
            }
            return asignacionTareasBD;
        }
        //--------------------------------METODO obtener todas las AsignacionTareas.--------------------------
        public static async Task<List<AsignacionTareas>> GetAllAsync()
        {
            var asignacionTareas = new List<AsignacionTareas>(); //una variable de lo que llevara una lista de AsignacionTareas
            using (var bdContexto = new ContextoBD()) //creo el acceso a la BD
            {
                asignacionTareas = await bdContexto.AsignacionTareas.ToListAsync(); //le digo que categories contenga la lista de AsignacionTareas, osea lo de l BD
            }
            return asignacionTareas;
        }
    }   
}
