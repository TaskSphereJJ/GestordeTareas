﻿using GestordeTaras.EN;
using GestordeTareas.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class TareaDAL
    {
        //--------------------------------METODO CREAR Tarea.--------------------------
        public static async Task<int> CreateAsync(Tarea tarea)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD()) //el comando using hace un proceso de ejecucion
            {
                dbContexto.Tarea.Add(tarea); //agrego una nueva tarea
                result = await dbContexto.SaveChangesAsync();//se guarda a la base de datos
            }
            return result;
        }

        //--------------------------------METODO MODIFICAR TArea.--------------------------
        public static async Task<int> UpdateAsync(Tarea tarea)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var tareaBD = await bdContexto.Tarea.FirstOrDefaultAsync(c => c.Id == tarea.Id);
                if (tareaBD != null)
                {
                    // Actualizar solo las propiedades que necesitan ser actualizadas
                    tareaBD.Nombre = tarea.Nombre;
                    tareaBD.Descripcion = tarea.Descripcion;
                    tareaBD.FechaCreacion = tarea.FechaCreacion;
                    tareaBD.FechaVencimiento = tarea.FechaVencimiento;
                    tareaBD.IdCategoria = tarea.IdCategoria;
                    tareaBD.IdPrioridad = tarea.IdPrioridad;
                    tareaBD.IdEstadoTarea = tarea.IdEstadoTarea;
                    tareaBD.IdProyecto = tarea.IdProyecto;


                    // Guardar cambios solo si hay propiedades actualizadas
                    bdContexto.Update(tareaBD);
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }

        //...............--------------METODO ELIMINAR---------------------------
        public static async Task<int> DeleteAsync(Tarea tarea)
        {

            int result = 0;
            using (var bdContexto = new ContextoBD()) //istancio la coneccion
            {
                var tareaBD = await bdContexto.Tarea.FirstOrDefaultAsync(t => t.Id == tarea.Id); //busco el id
                if (tareaBD != null)//verifico que no este nulo
                {
                    bdContexto.Tarea.Remove(tareaBD);//elimino anivel de memoria la tarea
                    result = await bdContexto.SaveChangesAsync();//le digo a la BD que se elimine y se guarde
                }
            }
            return result;
        }
        //--------------------------------METODO obtenerporID Tareas.--------------------------
        public static async Task<Tarea> GetByIdAsync(Tarea tarea)
        {
            var tareaBD = new Tarea();
            using (var bdContexto = new ContextoBD())
            {
                tareaBD = await bdContexto.Tarea
                    .Include(c => c.Categoria)
                    .Include(p => p.Prioridad)
                    .Include(e => e.EstadoTarea)
                    .Include(r => r.Proyecto)
                    .FirstOrDefaultAsync(t => t.Id == tarea.Id); //busco el id
            }
            return tareaBD;
        }

        //--------------------------------METODO obtener todas las tareas.--------------------------
        public static async Task<List<Tarea>> GetAllAsync()
        {
            using (var dbContext = new ContextoBD())
            {
                var tareas = await dbContext.Tarea
                    .Include(c => c.Categoria)
                    .Include(p => p.Prioridad)
                    .Include(e => e.EstadoTarea)
                    .Include(r => r.Proyecto)
                    .Include(et => et.ElegirTarea)
                    .ThenInclude(et => et.Usuario)


                    .ToListAsync();

                return tareas;
            }
        }

        // Filtra las tareas por el ID del proyecto proporcionado 
        public static async Task<List<Tarea>> GetTareasByProyectoIdAsync(int proyectoId)
        {
            try
            {
                using (var dbContext = new ContextoBD())
                {
                    var tareas = await dbContext.Tarea
                        .Include(c => c.Categoria)
                        .Include(p => p.Prioridad)
                        .Include(e => e.EstadoTarea)
                        .Include(r => r.Proyecto)
                        .Include(et => et.ElegirTarea)
                    .ThenInclude(et => et.Usuario)
                        .Where(t => t.IdProyecto == proyectoId)
                        .ToListAsync();

                    // Verificar el conteo de tareas
                    Debug.WriteLine($"Tareas encontradas: {tareas.Count}");

                    return tareas;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return new List<Tarea>(); // Retorna una lista vacía en caso de error
            }
        }


        //metodo para poder cambiar el estado de las tareas
        public static async Task<int> ActualizarEstadoTareaAsync(int idTarea, int idEstadoTarea)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())
            {
                var tareaBD = await bdContexto.Tarea.FirstOrDefaultAsync(t => t.Id == idTarea);
                if (tareaBD != null)
                {
                    //accedo a actualizar la tarea
                    tareaBD.IdEstadoTarea = idEstadoTarea;
                    bdContexto.Update(tareaBD);
                    //aqui se guardan los cambios
                    result = await bdContexto.SaveChangesAsync();
                }
            }
            return result;
        }
        public static async Task<List<Tarea>> GetTareasFinalizadas()
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.Tarea
                    .Include(c => c.Categoria)
                    .Include(p => p.Prioridad)
                    .Include(e => e.EstadoTarea)
                    .Include(r => r.Proyecto)
                    .Where(t => t.IdEstadoTarea == 3) 
                    .ToListAsync();
            }
        }

    }
}