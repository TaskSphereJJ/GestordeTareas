using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class ProyectoDAL
    {
        public async static Task<int> CreateAsync(GestordeTaras.EN.Proyecto proyecto)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD())
            {
                dbContexto.Proyecto.Add(proyecto);
                result = await dbContexto.SaveChangesAsync();
            }
            return result;
        }


        public static async  Task<int> UpdateAsync(GestordeTaras.EN.Proyecto proyecto)
        { 
            try
            {
                using (var dbContexto = new ContextoBD())
                {
                    var existingProyecto = await dbContexto.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);

                    if (existingProyecto != null)
                    {
                        // Actualizar las propiedades del proyecto existente con los valores del proyecto pasado como parámetro
                        existingProyecto.Titulo = proyecto.Titulo;
                        existingProyecto.Descripcion = proyecto.Descripcion;
                        existingProyecto.AdministradorID = proyecto.AdministradorID;
                        existingProyecto.CodigoAcceso = proyecto.CodigoAcceso;
                        existingProyecto.FechaFinalizacion = proyecto.FechaFinalizacion;

                        // Marcar la entidad como modificada para que Entity Framework realice el seguimiento de los cambios
                        dbContexto.Update(existingProyecto);

                        // Guardar los cambios en la base de datos
                        int result = await dbContexto.SaveChangesAsync();
                        return result;
                    }
                    else
                    {
                        // Manejar el caso en que el proyecto no existe
                        return 0; // O algún otro valor que indique que no se pudo encontrar el proyecto
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones, loguear, notificar, etc.
                Console.WriteLine($"Error en ac: {ex.Message}");
                return -1; // O algún otro valor que indique un error
            }
        }



        public static async Task<int> DeleteAsync(GestordeTaras.EN.Proyecto proyecto)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var proyectoDb = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);
                if (proyectoDb != null)
                {
                    dbContext.Proyecto.Remove(proyectoDb);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }


        public static async Task<GestordeTaras.EN.Proyecto> GetByIdAsync(int proyectoId)
        {
            using (var dbContexto = new ContextoBD())
            {
                var proyectoDb = await dbContexto.Proyecto.FirstOrDefaultAsync(p => p.Id == proyectoId);
                return proyectoDb!;
            }
        }

        public static async Task<List<GestordeTaras.EN.Proyecto>> GetAllAsync(GestordeTaras.EN.Proyecto proyecto)
        {
            var _proyectos = new List<GestordeTaras.EN.Proyecto>();
            using (var dbContexto = new ContextoBD())
            {
                _proyectos = await dbContexto.Proyecto.ToListAsync();
            }
            return _proyectos;
        }

        public static Task<List<GestordeTaras.EN.Proyecto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public static async Task<GestordeTaras.EN.Proyecto> GetByIdAsync(GestordeTaras.EN.Proyecto proyecto)
        {
            throw new NotImplementedException();
        }
    }
}
