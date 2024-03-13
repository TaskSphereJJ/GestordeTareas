using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class UsuarioDAL
    {
        // Método para crear un nuevo usuario en la base de datos de forma asincrónica.
        public static async Task<int> Create(Usuarios usuario)
        {
            int result = 0;
            using (var bdContext = new ContextoBD())
            {
                // Agrega el usuario al DbSet correspondiente en el contexto.
                bdContext.Add(usuario);
                // Guarda los cambios en la base de datos.
                await bdContext.SaveChangesAsync();
            }

            // Retorna el resultado (podría ser el ID del usuario recién creado, por ejemplo).
            return result;
        }

        // Método para actualizar un usuario existente en la base de datos de forma asincrónica.
        public static async Task<int> Update(Usuarios usuario)
        {
            int result = 0;
            using (var bdContext = new ContextoBD())
            {
                // Busca el usuario existente por su ID.
                var usuarioDB = await bdContext.Usuario.FirstOrDefaultAsync(a => a.Id == usuario.Id);
                if (usuarioDB != null)
                {
                    // Actualiza las propiedades del usuario con los nuevos valores.
                    usuarioDB.Nombre = usuario.Nombre;
                    usuarioDB.Apellido = usuario.Apellido;
                    usuarioDB.Email = usuario.Email;
                    usuarioDB.Pass = usuario.Pass;
                    usuarioDB.Telefono = usuario.Telefono;
                    usuarioDB.FechaNacimiento = usuario.FechaNacimiento;
                    usuarioDB.IdCargo = usuario.IdCargo;


                    // Marca el usuario como modificado en el contexto.
                    bdContext.Update(usuarioDB);
                    // Guarda los cambios en la base de datos.
                    result = await bdContext.SaveChangesAsync();
                }

                // Retorna el resultado (número de filas afectadas en la base de datos).
                return result;
            }
        }

        // Método para eliminar un usuario de la base de datos de forma asincrónica.
        public static async Task<int> Delete(Usuarios usuario)
        {
            int result = 0;
            using (var bdContext = new ContextoBD())
            {
                // Busca el usuario existente por su ID.
                var usuarioDB = await bdContext.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
                if (usuarioDB != null)
                {
                    // Elimina el usuario del DbSet correspondiente en el contexto.
                    bdContext.Usuario.Remove(usuarioDB);
                    // Guarda los cambios en la base de datos.
                    result = await bdContext.SaveChangesAsync();
                }
            }
            // Retorna el resultado (número de filas afectadas en la base de datos).
            return result;
        }

        // Método para obtener un usuario por su ID de forma asincrónica.
        public static async Task<Usuarios> GetByIdAsync(Usuarios usuario)
        {
            var usuarioDB = new Usuarios();
            using (var bdContexto = new ContextoBD())
            {
                // Busca el usuario por su ID y asigna el resultado a la variable usuarioDB.
                usuarioDB = await bdContexto.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            }
            // Retorna el usuario encontrado.
            return usuarioDB;
        }

        // Método para obtener todos los usuarios de la base de datos de forma asincrónica.
        public static async Task<List<Usuarios>> GetAllAsync()
        {
            var usuarios = new List<Usuarios>();
            using (var bdContexto = new ContextoBD())
            {
                // Obtiene todos los usuarios y los asigna a la variable usuarios.
                usuarios = await bdContexto.Usuario.ToListAsync();
            }
            // Retorna la lista de usuarios.
            return usuarios;
        }
    }


}
