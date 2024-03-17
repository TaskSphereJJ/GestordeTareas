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
        private static void EncryptMD5(Usuarios user)
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

        private static async Task<bool> ExistsLogin(Usuarios user, ContextoBD context)
        {
            bool result = false;
            var userLoginExists = await context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == user.NombreUsuario && u.Id != user.Id);
            if (userLoginExists != null && userLoginExists.Id > 0 && userLoginExists.NombreUsuario == user.NombreUsuario)
                result = true;

            return result;
        }

        // Método para crear un nuevo usuario en la base de datos de forma asincrónica.
        public static async Task<int> Create(Usuarios usuario)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                bool existsLogin = await ExistsLogin(usuario, dbContext);
                if (existsLogin == false)
                {
                    usuario.RegistrationDate = DateTime.Now;
                    EncryptMD5(usuario);
                    dbContext.Usuarios.Add(usuario);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }

        // Método para actualizar un usuario existente en la base de datos de forma asincrónica.
        public static async Task<int> Update(Usuarios usuario)
        {
            int result = 0;
            using (var dbContext = new ContextoBD())
            {
                bool existsLogin = await ExistsLogin(usuario, dbContext);
                if (existsLogin == false)
                {
                    var userDb = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == usuario.Id);
                    userDb.CargoId = usuario.CargoId;
                    userDb.Nombre = usuario.Nombre;
                    userDb.Apellido = usuario.Apellido;
                    userDb.Status = usuario.Status;
                    userDb.NombreUsuario = usuario.NombreUsuario;

                    dbContext.Usuarios.Update(userDb);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }

        // Método para eliminar un usuario de la base de datos de forma asincrónica.
        public static async Task<int> Delete(Usuarios usuario)
        {
            int result = 0;
            using (var bdContext = new ContextoBD())
            {
                // Busca el usuario existente por su ID.
                var usuarioDB = await bdContext.Usuarios.FirstOrDefaultAsync(u => u.Id == usuario.Id);
                if (usuarioDB != null)
                {
                    // Elimina el usuario del DbSet correspondiente en el contexto.
                    bdContext.Usuarios.Remove(usuarioDB);
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
                usuarioDB = await bdContexto.Usuarios.FirstOrDefaultAsync(u => u.Id == usuario.Id);
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
                usuarios = await bdContexto.Usuarios.ToListAsync();
            }
            // Retorna la lista de usuarios.
            return usuarios;
        }

        internal static IQueryable<Usuarios> QuerySelect(IQueryable<Usuarios> query, Usuarios user)
        {
            if (user.Id > 0)
                query = query.Where(u => u.Id == user.Id);

            if (user.CargoId > 0)
                query = query.Where(u => u.CargoId == user.CargoId);

            if (!string.IsNullOrWhiteSpace(user.Nombre))
                query = query.Where(u => u.Nombre.Contains(user.Nombre));

            if (!string.IsNullOrWhiteSpace(user.Apellido))
                query = query.Where(u => u.Apellido.Contains(user.Apellido));

            if (!string.IsNullOrWhiteSpace(user.NombreUsuario))
                query = query.Where(u => u.NombreUsuario.Contains(user.NombreUsuario));

            if (user.Status > 0)
                query = query.Where(u => u.Status == user.Status);

            if (user.RegistrationDate.Year > 1000)
            {
                DateTime inicialDate = new DateTime(user.RegistrationDate.Year, user.RegistrationDate.Month, user.RegistrationDate.Day, 0, 0, 0);
                DateTime finalDate = inicialDate.AddDays(1).AddMilliseconds(-1);
                query = query.Where(u => u.RegistrationDate >= inicialDate && u.RegistrationDate <= finalDate);
            }

            query = query.OrderByDescending(u => u.Id).AsQueryable();

            if (user.Top_Aux > 0)
                query = query.Take(user.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<Usuarios>> SearchAsync(Usuarios usuarios)
        {
            var users = new List<Usuarios>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.Usuarios.AsQueryable();
                select = QuerySelect(select, usuarios);
                users = await select.ToListAsync();
            }
            return users;
        }

        public static async Task<List<Usuarios>> SearchIncludeRoleAsync(Usuarios user)
        {
            var users = new List<Usuarios>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.Usuarios.AsQueryable();
                select = QuerySelect(select, user).Include(u => u.Cargo).AsQueryable();
                users = await select.ToListAsync();
            }
            return users;
        }

        public static async Task<Usuarios> LoginAsync(Usuarios usuarios)
        {
            var userDb = new Usuarios();
            using (var dbContext = new ContextoBD())
            {
                EncryptMD5(usuarios);
                userDb = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == usuarios.NombreUsuario &&
                u.Pass == usuarios.Pass && u.Status == (byte)User_Status.ACTIVO);
            }
            return userDb!;
        }

        public static async Task<int> ChangePasswordAsync(Usuarios user, string oldPassword)
        {
            int result = 0;
            var userOldPass = new Usuarios { Pass = oldPassword };
            EncryptMD5(userOldPass);
            using (var dbContext = new ContextoBD())
            {
                var userDb = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (userOldPass.Pass == userDb.Pass)
                {
                    EncryptMD5(user);
                    userDb.Pass = user.Pass;
                    dbContext.Usuarios.Update(userDb);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("La contraseña actual es inválida");
            }
            return result;
        }
    }


}
