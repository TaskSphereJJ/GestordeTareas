using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class ProyectoDAL
    {

        public static async Task<int> CreateAsync(Proyecto proyecto)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                ////proyecto.IdUsuario = idUsuario; // Asignar el IdUsuario al proyecto
                dbContext.Proyecto.Add(proyecto);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }


        public static async Task<int> UpdateAsync(Proyecto proyecto)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var existingProyecto = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);

                if (existingProyecto != null)
                {
                    existingProyecto.Titulo = proyecto.Titulo;
                    existingProyecto.Descripcion = proyecto.Descripcion;
                    existingProyecto.IdUsuario = proyecto.IdUsuario;
                    existingProyecto.FechaFinalizacion = proyecto.FechaFinalizacion;
                    existingProyecto.CodigoAcceso = proyecto.CodigoAcceso;

                    dbContext.Update(existingProyecto);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<int> DeleteAsync(Proyecto proyecto)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                var existingProyecto = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);
                if (existingProyecto != null)
                {
                    dbContext.Proyecto.Remove(existingProyecto);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<Proyecto> GetByIdAsync(Proyecto proyecto)
        {
            using (var bdContexto = new ContextoBD())
            {
                // Buscar el proyecto por su ID y cargar la propiedad de navegación Usuario si es necesario
                var projectBD = await bdContexto.Proyecto
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == proyecto.Id);

                // Manejar el caso cuando no se encuentra ningún proyecto
                if (projectBD == null)
                {
                    // Puedes lanzar una excepción, retornar null u otro valor según tu lógica de negocio
                    throw new Exception("El proyecto no existe en la base de datos.");
                }

                return projectBD;
            }
        }

        public static async Task<List<Proyecto>> GetAllAsync()
        {
            using (var dbContext = new ContextoBD())
            {
                var proyectos = await dbContext.Proyecto.Include(p => p.Usuario).ToListAsync();
                return proyectos;
            }
        }

        //Método para generar un codigo de acceso para proyectos
        public static string GenerarCodigoAcceso()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var codigo = new char[8]; // Longitud del código
            for (int i = 0; i < codigo.Length; i++)
            {
                codigo[i] = caracteres[random.Next(caracteres.Length)];
            }
            return new string(codigo);
        }

        // Método para verificar si el código de acceso ya existe en la base de datos
        public static async Task<bool> ExisteCodigoAccesoAsync(string codigoAcceso)
        {
            using (var dbContext = new ContextoBD())
            {
                return await dbContext.Proyecto.AnyAsync(p => p.CodigoAcceso == codigoAcceso);
            }
        }
    }
}
