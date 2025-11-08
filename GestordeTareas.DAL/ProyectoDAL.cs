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
        // Crear un proyecto
        public static async Task<int> CreateAsync(Proyecto proyecto)
        {
            using var dbContext = new ContextoBD();
            await dbContext.Proyecto.AddAsync(proyecto);
            return await dbContext.SaveChangesAsync();
        }

        // Actualizar un proyecto
        public static async Task<int> UpdateAsync(Proyecto proyecto)
        {
            using var dbContext = new ContextoBD();
            var proyectoDB = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);

            if (proyectoDB == null)
                return 0;

            proyectoDB.Titulo = proyecto.Titulo;
            proyectoDB.Descripcion = proyecto.Descripcion;
            proyectoDB.IdUsuario = proyecto.IdUsuario;
            proyectoDB.FechaFinalizacion = proyecto.FechaFinalizacion;
            proyectoDB.CodigoAcceso = proyecto.CodigoAcceso;

            dbContext.Proyecto.Update(proyectoDB);
            return await dbContext.SaveChangesAsync();
        }

        // Eliminar un proyecto
        public static async Task<int> DeleteAsync(Proyecto proyecto)
        {
            using var dbContext = new ContextoBD();
            var proyectoDB = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);

            if (proyectoDB == null)
                return 0;

            dbContext.Proyecto.Remove(proyectoDB);
            return await dbContext.SaveChangesAsync();
        }

        // Obtener proyecto por ID
        public static async Task<Proyecto> GetByIdAsync(Proyecto proyecto)
        {
            using var dbContext = new ContextoBD();
            var proyectoDB = await dbContext.Proyecto
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == proyecto.Id);

            if (proyectoDB == null)
                throw new Exception("El proyecto no existe en la base de datos.");

            return proyectoDB;
        }

        // Obtener todos los proyectos
        public static async Task<List<Proyecto>> GetAllAsync()
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Proyecto
                .Include(p => p.Usuario)
                .Include(p => p.ProyectoUsuario)
                    .ThenInclude(pu => pu.Usuario)
                .ToListAsync();
        }

        // Generar código de acceso único
        public static string GenerarCodigoAcceso()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var codigo = new char[8]; // Longitud del código

            for (int i = 0; i < codigo.Length; i++)
                codigo[i] = caracteres[random.Next(caracteres.Length)];

            return new string(codigo);
        }

        // Verificar si un código de acceso existe
        public static async Task<bool> ExisteCodigoAccesoAsync(string codigoAcceso)
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Proyecto.AnyAsync(p => p.CodigoAcceso == codigoAcceso);
        }

        // Buscar proyecto por título o nombre del administrador
        public static async Task<List<Proyecto>> BuscarPorTituloOAdministradorAsync(string query)
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Proyecto
                .Include(p => p.Usuario)
                .Where(p => p.Titulo.Contains(query) || p.Usuario.Nombre.Contains(query))
                .ToListAsync();
        }
    }
}





























//using GestordeTaras.EN;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GestordeTareas.DAL
//{
//    public class ProyectoDAL
//    {

//        //METODO PARA CREAR UN PROYECTO
//        public static async Task<int> CreateAsync(Proyecto proyecto)
//        {
//            int result = 0;
//            using (var dbContext = new ContextoBD())
//            {
//                dbContext.Proyecto.Add(proyecto);
//                result = await dbContext.SaveChangesAsync();
//            }
//            return result;
//        }


//        //METODO PARA MODIFICAR UN PROYECTO
//        public static async Task<int> UpdateAsync(Proyecto proyecto)
//        {
//            int result = 0;
//            using (var dbContext = new ContextoBD())
//            {
//                var existingProyecto = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);

//                if (existingProyecto != null)
//                {
//                    existingProyecto.Titulo = proyecto.Titulo;
//                    existingProyecto.Descripcion = proyecto.Descripcion;
//                    existingProyecto.IdUsuario = proyecto.IdUsuario;
//                    existingProyecto.FechaFinalizacion = proyecto.FechaFinalizacion;
//                    existingProyecto.CodigoAcceso = proyecto.CodigoAcceso;

//                    dbContext.Update(existingProyecto);
//                    result = await dbContext.SaveChangesAsync();
//                }
//            }
//            return result;
//        }


//        //METODO PARA ELIMINAR UN PROYECTO
//        public static async Task<int> DeleteAsync(Proyecto proyecto)
//        {
//            int result = 0;
//            using (var dbContext = new ContextoBD())
//            {
//                var existingProyecto = await dbContext.Proyecto.FirstOrDefaultAsync(p => p.Id == proyecto.Id);
//                if (existingProyecto != null)
//                {
//                    dbContext.Proyecto.Remove(existingProyecto);
//                    result = await dbContext.SaveChangesAsync();
//                }
//            }
//            return result;
//        }


//        //METODO PARA OBTENER UN PROYECTO POR ID
//        public static async Task<Proyecto> GetByIdAsync(Proyecto proyecto)
//        {
//            using (var bdContexto = new ContextoBD())
//            {
//                var projectBD = await bdContexto.Proyecto
//                    .Include(p => p.Usuario)
//                    .FirstOrDefaultAsync(p => p.Id == proyecto.Id);

//                if (projectBD == null)
//                {
//                    throw new Exception("El proyecto no existe en la base de datos.");
//                }

//                return projectBD;
//            }
//        }


//        //METODO PARA OBTENER TODOS LOS PROYECTOS
//        public static async Task<List<Proyecto>> GetAllAsync()
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                var proyectos = await dbContext.Proyecto
//                            .Include(p => p.Usuario)
//                            .Include(p => p.ProyectoUsuario) 
//                                .ThenInclude(pu => pu.Usuario) 
//                            .ToListAsync();
//                return proyectos;
//            }
//        }


//        //MÉTODO PARA GENERAR UN CODIGO DE ACCESO PARA PROYECTOS
//        public static string GenerarCodigoAcceso()
//        {
//            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//            var random = new Random();
//            var codigo = new char[8]; // Longitud del código
//            for (int i = 0; i < codigo.Length; i++)
//            {
//                codigo[i] = caracteres[random.Next(caracteres.Length)];
//            }
//            return new string(codigo);
//        }


//        // MÉTODO PARA VERIFICAR SI EL CÓDIGO DE ACCESO YA EXISTE EN LA BASE DE DATOS
//        public static async Task<bool> ExisteCodigoAccesoAsync(string codigoAcceso)
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                return await dbContext.Proyecto.AnyAsync(p => p.CodigoAcceso == codigoAcceso);
//            }
//        }


//        // MÉTODO PARA BUSCAR PROYECTO POR TÍTULO O NOMBRE DEL ADMINISTRADOR
//        public static async Task<List<Proyecto>> BuscarPorTituloOAdministradorAsync(string query)
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                var proyectos = await dbContext.Proyecto
//            .Include(p => p.Usuario) // Relación con Usuario
//            .Where(p => p.Titulo.Contains(query) ||
//                        p.Usuario.Nombre.Contains(query)) // Búsqueda por título o nombre del administrador
//            .ToListAsync();

//                return proyectos;
//            }
//        }


//    }
//}
