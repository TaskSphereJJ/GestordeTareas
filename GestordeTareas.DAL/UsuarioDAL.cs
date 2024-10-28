﻿using GestordeTaras.EN;
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
        //METODO QUE SE ENCARGA DE ENCRIPTAR LA CONTRASEÑA DEL USUARIO
        private static void EncryptMD5(Usuario user)
        {
            // Crea una instancia del algoritmo MD5
            using (var md5 = MD5.Create())
            {
                // Convierte la contraseña a un arreglo de bytes usando la codificación ASCII
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(user.Pass));

                // Convierte el arreglo de bytes resultante en una cadena hexadecimal
                var encryptedStr = "";
                for (int i = 0; i < result.Length; i++)
                {
                    encryptedStr += result[i].ToString("x2").ToLower();
                }
                // Asigna la contraseña cifrada al objeto Usuario
                user.Pass = encryptedStr;
            }
        }

        //METODO QUE COMPARA LA CONTRASEÑA INGRESADA CON LA ALMACENADA DEL USUARIO PARA PERMITIR MODIIFCARLA
        public static string HashMD5(string password)
        {
            // Crea una instancia del algoritmo MD5.
            using (var md5 = MD5.Create())
            {
                // Convierte la cadena de texto (contraseña) a un arreglo de bytes usando la codificación ASCII.
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

                // Convierte el arreglo de bytes resultante en una cadena hexadecimal.
                var encryptedStr = "";
                for (int i = 0; i < result.Length; i++)
                {
                    encryptedStr += result[i].ToString("x2").ToLower();
                }
                // Devuelve la contraseña cifrada en formato de cadena hexadecimal.
                return encryptedStr;
            }
        }


        //METODO PARA VERIFICAR SI EL NOMBRE DE USUARIO YA EXISTE
        private static async Task<bool> ExistsLogin(Usuario user, ContextoBD context)
        {
            bool result = false;
            // Consulta en la base de datos si ya existe un usuario con el mismo nombre de usuario pero con un ID diferente.
            var userLoginExists = await context.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == user.NombreUsuario && u.Id != user.Id);

            // Si se encuentra un usuario que cumpla con las condiciones, el método devolverá 'true'.
            if (userLoginExists != null && userLoginExists.Id > 0 && userLoginExists.NombreUsuario == user.NombreUsuario)
                result = true;

            return result;
        }


        // MÉTODO PARA CREAR UN NUEVO USUARIO 
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

                    if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                    {
                        usuario.FotoPerfil = usuario.FotoPerfil;
                    }

                    dbContext.Usuario.Add(usuario);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }


        // MÉTODO PARA ACTUALIZAR UN USUARIO EXISTENTE 
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
                    userDb.Telefono = usuario.Telefono;
                    userDb.Pass = usuario.Pass;
                    userDb.FechaNacimiento = usuario.FechaNacimiento;
                    userDb.FechaRegistro = usuario.FechaRegistro;
                    userDb.NombreUsuario = usuario.NombreUsuario;
                    // Actualiza la foto de perfil solo si no es nula
                    if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                    {
                        userDb.FotoPerfil = usuario.FotoPerfil;
                    }
                    // Solo actualiza la contraseña si se ha proporcionado una nueva
                    if (!string.IsNullOrEmpty(usuario.Pass) && usuario.Pass != userDb.Pass)
                    {
                        EncryptMD5(usuario); // Encripta la nueva contraseña
                        userDb.Pass = usuario.Pass; // Se asigna la contraseña encriptada
                    }
                    else
                    {
                        // Si no se proporciona una nueva contraseña, se mantiene la existente
                        userDb.Pass = userDb.Pass; // No se cambia la contraseña
                    }
                    dbContext.Usuario.Update(userDb);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }


        // MÉTODO PARA ELIMINAR UN USUARIO 
        public static async Task<int> Delete(Usuario usuario)

        {
            // Se verifica si el usuario tiene referencias y se obtiene un mensaje si es así.
            string mensajeReferencia = await TieneReferencias(usuario.Id);
            if (mensajeReferencia != null)
            {
                throw new Exception(mensajeReferencia); // Lanza una excepción con el mensaje específico.
            }

            int result = 0;
            using (var bdContext = new ContextoBD())
            {
                // Busca el usuario existente por su ID.
                var usuarioDB = await bdContext.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
                if (usuarioDB != null)
                {
                    bdContext.Usuario.Remove(usuarioDB);
                    result = await bdContext.SaveChangesAsync();
                }
            }
            // Retorna el resultado (número de filas afectadas en la base de datos).
            return result;
        }


        // MÉTODO PARA OBTENER UN USUARIO POR SU ID 
        public static async Task<Usuario> GetByIdAsync(Usuario usuario)
        {
            var usuarioDB = new Usuario();
            using (var bdContexto = new ContextoBD())
            {
                // Busca el usuario por su ID y asigna el resultado a la variable usuarioDB.
                usuarioDB = await bdContexto.Usuario.Include(u => u.Cargo).FirstOrDefaultAsync(u => u.Id == usuario.Id);
            }
            // Retorna el usuario encontrado.
            return usuarioDB;
        }


        // MÉTODO PARA OBTENER UN USUARIO POR SU NOMBRE DE USUARIO 
        public static async Task<Usuario> GetByNombreUsuarioAsync(Usuario usuario)
        {
            using (var dbContext = new ContextoBD())
            {
                // Busca el usuario por su nombre de usuario.
                var usuarioDB = await dbContext.Usuario
                    .Include(u => u.Cargo) // Incluir información de Cargo si es necesario
                    .FirstOrDefaultAsync(u => u.NombreUsuario == usuario.NombreUsuario);
                return usuarioDB;
            }
        }


        // MÉTODO PARA OBTENER TODOS LOS USUARIOS 
        public static async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();
            using (var bdContexto = new ContextoBD())
            {
                // Obtiene todos los usuarios y los asigna a la variable usuarios.
                usuarios = await bdContexto.Usuario.Include(c => c.Cargo).ToListAsync();
            }
            // Retorna la lista de usuarios
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
                select = QuerySelect(select, usuarios).Include(u => u.Cargo); 
                users = await select.ToListAsync();
            }
            return users;
        }


        public static async Task<List<Usuario>> SearchIncludeRoleAsync(Usuario user, string query, string filter)
        {
            var users = new List<Usuario>();
            using (var dbContext = new ContextoBD())
            {
                var select = dbContext.Usuario.AsQueryable();
                // Lógica para filtrar según el parámetro
                if (!string.IsNullOrEmpty(query))
                {
                    if (filter == "Apellido")
                    {
                        select = select.Where(u => u.Apellido.Contains(query));
                    }
                    else if (filter == "NombreUsuario")
                    {
                        select = select.Where(u => u.NombreUsuario.Contains(query));
                    }
                }

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


        //ESTE METODO ES PARA VERIFICAR SI EL USUARIO TIENE REFERENCIAS EN OTRAS TABLAS
        public static async Task<string> TieneReferencias(int usuarioId)
        {
            using (var dbContext = new ContextoBD())
            {
                // Verifica si el usuario tiene referencias en la tabla InvitacionProyecto
                bool tieneInvitaciones = await dbContext.InvitacionProyecto.AnyAsync(ip => ip.IdUsuario == usuarioId);
                if (tieneInvitaciones)
                {
                    return "Este usuario tiene invitaciones pendientes en proyectos.";
                }

                // Verifica si el usuario tiene referencias en la tabla ProyectoUsuario
                bool tieneProyectos = await dbContext.ProyectoUsuario.AnyAsync(pu => pu.IdUsuario == usuarioId);
                if (tieneProyectos)
                {
                    return "Este usuario está asociado a un proyecto.";
                }

                // Si no tiene referencias
                return null;
            }
        }


    }
}