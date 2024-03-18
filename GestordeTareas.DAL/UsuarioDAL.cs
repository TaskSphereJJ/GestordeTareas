using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class UsuarioDAL
    {
        private static void EncryptMD5(Usuario user)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(user.Pass));
                var encryptedStr = "";
                for (int i = 0; i < result.Length; i++)
                {
                    encryptedStr += result[i].ToString("x2").ToLower();
                }
                user.Pass = encryptedStr;
            }
        }

        private static async Task<bool> ExistsLogin(Usuario user, ContextoBD context)
        {
            bool result = false;
            var userLoginExists = await context.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == user.NombreUsuario && u.Id != user.Id);
            if (userLoginExists != null && userLoginExists.Id > 0 && userLoginExists.NombreUsuario == user.NombreUsuario)
                result = true;

            return result;
        }

        // Método para crear un nuevo usuario en la base de datos de forma asincrónica.
        public static async Task<int> Create(Usuario usuario)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                bool existsLogin = await ExistsLogin(usuario, dbContext);
                if (existsLogin == false)
                {
                    usuario.FechaRegistro = DateTime.Now;
                    EncryptMD5(usuario);
                    dbContext.Usuario.Add(usuario);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }

        // Método para actualizar un usuario existente en la base de datos de forma asincrónica.
        public static async Task<int> Update(Usuario usuario)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                bool existsLogin = await ExistsLogin(usuario, dbContext);
                if (existsLogin == false)
                {
                    var userDb = await dbContext.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
                    userDb.IdCargo = usuario.IdCargo;
                    userDb.Nombre = usuario.Nombre;
                    userDb.Apellido = usuario.Apellido;
                    userDb.Status = usuario.Status;
                    userDb.NombreUsuario = usuario.NombreUsuario;

                    dbContext.Usuario.Update(userDb);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }

        // Método para eliminar un usuario de la base de datos de forma asincrónica.
        public static async Task<int> Delete(Usuario usuario)
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
        public static async Task<Usuario> GetByIdAsync(Usuario usuario)
        {
            var usuarioDB = new Usuario();
            using (var bdContexto = new ContextoBD())
            {
                // Busca el usuario por su ID y asigna el resultado a la variable usuarioDB.
                usuarioDB = await bdContexto.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            }
            // Retorna el usuario encontrado.
            return usuarioDB;
        }

        // Método para obtener todos los usuarios de la base de datos de forma asincrónica.
        public static async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();
            using (var bdContexto = new ContextoBD())
            {
                // Obtiene todos los usuarios y los asigna a la variable usuarios.
                usuarios = await bdContexto.Usuario.ToListAsync();
            }
            // Retorna la lista de usuarios.
            return usuarios;
        }

        internal static IQueryable<Usuario> QuerySelect(IQueryable<Usuario> query, Usuario user)
        {
            if (user.Id > 0)
                query = query.Where(u => u.Id == user.Id);

            if (user.IdCargo > 0)
                query = query.Where(u => u.IdCargo == user.IdCargo);

            if (!string.IsNullOrWhiteSpace(user.Nombre))
                query = query.Where(u => u.Nombre.Contains(user.Nombre));

            if (!string.IsNullOrWhiteSpace(user.Apellido))
                query = query.Where(u => u.Apellido.Contains(user.Apellido));

            if (!string.IsNullOrWhiteSpace(user.NombreUsuario))
                query = query.Where(u => u.NombreUsuario.Contains(user.NombreUsuario));

            if (user.Status > 0)
                query = query.Where(u => u.Status == user.Status);

            if (user.FechaRegistro.Year > 1000)
            {
                DateTime inicialDate = new DateTime(user.FechaRegistro.Year, user.FechaRegistro.Month, user.FechaRegistro.Day, 0, 0, 0);
                DateTime finalDate = inicialDate.AddDays(1).AddMilliseconds(-1);
                query = query.Where(u => u.FechaRegistro >= inicialDate && u.FechaRegistro <= finalDate);
            }

            query = query.OrderByDescending(u => u.Id).AsQueryable();

            if (user.Top_Aux > 0)
                query = query.Take(user.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<Usuario>> SearchAsync(Usuario usuarios)
        {
            var users = new List<Usuario>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.Usuario.AsQueryable();
                select = QuerySelect(select, usuarios);
                users = await select.ToListAsync();
            }
            return users;
        }

        public static async Task<List<Usuario>> SearchIncludeRoleAsync(Usuario user)
        {
            var users = new List<Usuario>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.Usuario.AsQueryable();
                select = QuerySelect(select, user).Include(u => u.Cargo).AsQueryable();
                users = await select.ToListAsync();
            }
            return users;
        }

        public static async Task<Usuario> LoginAsync(Usuario usuarios)
        {
            var userDb = new Usuario();
            using (var dbContext = new ContextoBD())
            {
                EncryptMD5(usuarios);
                userDb = await dbContext.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == usuarios.NombreUsuario &&
                u.Pass == usuarios.Pass && u.Status == (byte)User_Status.ACTIVO);
            }
            return userDb!;
        }

        public static async Task<int> ChangePasswordAsync(Usuario user, string oldPassword)
        {
            int result = 0;
            var userOldPass = new Usuario { Pass = oldPassword };
            EncryptMD5(userOldPass);
            using (var dbContext = new ContextoBD())
            {
                var userDb = await dbContext.Usuario.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (userOldPass.Pass == userDb.Pass)
                {
                    EncryptMD5(user);
                    userDb.Pass = user.Pass;
                    dbContext.Usuario.Update(userDb);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("La contraseña actual es inválida");
            }
            return result;
        }
    }


}
