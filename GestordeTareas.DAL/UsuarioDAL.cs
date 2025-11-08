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
        // Genera un hash MD5 de una cadena de texto
        public static string HashMD5(string input)
        {
            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            return string.Concat(hashBytes.Select(b => b.ToString("x2").ToLower()));
        }

        // Encripta la contraseña del usuario usando MD5
        private static void EncryptMD5(Usuario user)
        {
            user.Pass = HashMD5(user.Pass);
        }

        // Verifica si el nombre de usuario o teléfono ya existen
        private static async Task<bool> ExistsLoginAsync(Usuario user, ContextoBD context)
        {
            bool loginExists = await context.Usuario.AnyAsync(u =>
                (u.NombreUsuario == user.NombreUsuario || u.Telefono == user.Telefono) && u.Id != user.Id);
            return loginExists;
        }

        // Crear un nuevo usuario
        public static async Task<int> CreateAsync(Usuario usuario)
        {
            using var dbContext = new ContextoBD();

            bool exists = await ExistsLoginAsync(usuario, dbContext);
            if (exists)
                throw new Exception("El correo electrónico o número de teléfono ya pertenece a un usuario.");

            usuario.FechaRegistro = DateTime.Now;
            EncryptMD5(usuario);

            dbContext.Usuario.Add(usuario);
            return await dbContext.SaveChangesAsync();
        }

        // Actualizar un usuario existente
        public static async Task<int> UpdateAsync(Usuario usuario)
        {
            using var dbContext = new ContextoBD();

            if (await ExistsLoginAsync(usuario, dbContext))
                throw new Exception("El correo electrónico o teléfono ya está registrado.");

            var userDb = await dbContext.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            if (userDb == null) throw new Exception("Usuario no encontrado.");

            userDb.IdCargo = usuario.IdCargo;
            userDb.Nombre = usuario.Nombre;
            userDb.Apellido = usuario.Apellido;
            userDb.Status = usuario.Status;
            userDb.Telefono = usuario.Telefono;
            userDb.FechaNacimiento = usuario.FechaNacimiento;
            userDb.NombreUsuario = usuario.NombreUsuario;

            if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                userDb.FotoPerfil = usuario.FotoPerfil;

            if (!string.IsNullOrEmpty(usuario.Pass) && usuario.Pass != userDb.Pass)
            {
                EncryptMD5(usuario);
                userDb.Pass = usuario.Pass;
            }

            dbContext.Usuario.Update(userDb);
            return await dbContext.SaveChangesAsync();
        }

        // Eliminar un usuario
        public static async Task<int> DeleteAsync(Usuario usuario)
        {
            string mensajeReferencia = await TieneReferenciasAsync(usuario.Id);
            if (mensajeReferencia != null)
                throw new Exception(mensajeReferencia);

            using var dbContext = new ContextoBD();
            var usuarioDB = await dbContext.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            if (usuarioDB == null) return 0;

            dbContext.Usuario.Remove(usuarioDB);
            return await dbContext.SaveChangesAsync();
        }

        // Obtener un usuario por ID
        public static async Task<Usuario> GetByIdAsync(Usuario usuario)
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Usuario
                .Include(u => u.Cargo)
                .FirstOrDefaultAsync(u => u.Id == usuario.Id);
        }

        // Obtener un usuario por nombre de usuario
        public static async Task<Usuario> GetByNombreUsuarioAsync(Usuario usuario)
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Usuario
                .Include(u => u.Cargo)
                .FirstOrDefaultAsync(u => u.NombreUsuario == usuario.NombreUsuario);
        }

        // Obtener todos los usuarios
        public static async Task<List<Usuario>> GetAllAsync()
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Usuario
                .Include(c => c.Cargo)
                .ToListAsync();
        }

        // Filtro dinámico
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
                DateTime ini = user.FechaRegistro.Date;
                DateTime fin = ini.AddDays(1).AddMilliseconds(-1);
                query = query.Where(u => u.FechaRegistro >= ini && u.FechaRegistro <= fin);
            }

            query = query.OrderByDescending(u => u.Id);
            if (user.Top_Aux > 0)
                query = query.Take(user.Top_Aux);

            return query;
        }

        // Búsqueda genérica
        public static async Task<List<Usuario>> SearchAsync(Usuario usuario)
        {
            using var dbContext = new ContextoBD();
            var select = QuerySelect(dbContext.Usuario.AsQueryable(), usuario)
                .Include(u => u.Cargo);
            return await select.ToListAsync();
        }

        // Búsqueda con filtro y rol
        public static async Task<List<Usuario>> SearchIncludeRoleAsync(Usuario user, string query, string filter)
        {
            using var dbContext = new ContextoBD();
            var select = dbContext.Usuario.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                if (filter == "Apellido")
                    select = select.Where(u => u.Apellido.Contains(query));
                else if (filter == "NombreUsuario")
                    select = select.Where(u => u.NombreUsuario.Contains(query));
            }

            select = QuerySelect(select, user).Include(u => u.Cargo);
            return await select.ToListAsync();
        }

        // Login de usuario
        public static async Task<Usuario> LoginAsync(Usuario usuario)
        {
            EncryptMD5(usuario);
            using var dbContext = new ContextoBD();
            return await dbContext.Usuario.FirstOrDefaultAsync(u =>
                u.NombreUsuario == usuario.NombreUsuario &&
                u.Pass == usuario.Pass &&
                u.Status == (byte)User_Status.ACTIVO);
        }

        // Verifica si un usuario tiene referencias en otras tablas
        public static async Task<string> TieneReferenciasAsync(int usuarioId)
        {
            using var dbContext = new ContextoBD();

            bool tieneProyectos = await dbContext.ProyectoUsuario.AnyAsync(pu => pu.IdUsuario == usuarioId);
            if (tieneProyectos)
                return "Este usuario está asociado a un proyecto.";

            return null;
        }

        public static async Task<int> GenerarCodigoRestablecimientoAsync(Usuario usuario)
        {
            int codigo = new Random().Next(100000, 999999);
            string codigoEncriptado = HashMD5(codigo.ToString());

            var resetCode = new PasswordResetCode
            {
                IdUsuario = usuario.Id,
                Codigo = codigoEncriptado,
                Expiration = DateTime.Now.AddMinutes(15)
            };

            try
            {
                using var dbContext = new ContextoBD();
                dbContext.PasswordResetCode.Add(resetCode);
                await dbContext.SaveChangesAsync();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("PasswordResetCode"))
                {
                    throw new Exception("Error: La tabla 'PasswordResetCode' no existe en la base de datos. Verifica la migración o la estructura del esquema.");
                }
                throw; // relanza si no es ese error
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el código de restablecimiento: {ex.Message}", ex);
            }

            return codigo;
        }


        // Validar código de restablecimiento
        public static async Task<bool> ValidarCodigoRestablecimientoAsync(int idUsuario, string codigo)
        {
            string codigoHash = HashMD5(codigo);
            using var dbContext = new ContextoBD();

            var resetCode = await dbContext.PasswordResetCode
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario && c.Codigo == codigoHash);

            return resetCode != null && resetCode.Expiration >= DateTime.Now;
        }

        // Restablecer contraseña
        public static async Task<int> RestablecerContrasenaAsync(int idUsuario, string codigo, string nuevaContrasena)
        {
            string codigoHash = HashMD5(codigo);
            using var dbContext = new ContextoBD();

            var resetCode = await dbContext.PasswordResetCode
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario && c.Codigo == codigoHash);

            if (resetCode == null || resetCode.Expiration < DateTime.Now)
                throw new Exception("Token inválido o expirado.");

            var usuario = await dbContext.Usuario.FindAsync(idUsuario);
            usuario.Pass = HashMD5(nuevaContrasena);

            dbContext.PasswordResetCode.Remove(resetCode);
            dbContext.Usuario.Update(usuario);
            return await dbContext.SaveChangesAsync();
        }
    }
}



































































//using GestordeTaras.EN;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace GestordeTareas.DAL
//{
//    public class UsuarioDAL
//    {
//        //METODO QUE SE ENCARGA DE ENCRIPTAR LA CONTRASEÑA DEL USUARIO
//        private static void EncryptMD5(Usuario user)
//        {
//            // Crea una instancia del algoritmo MD5
//            using (var md5 = MD5.Create())
//            {
//                // Convierte la contraseña a un arreglo de bytes usando la codificación ASCII
//                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(user.Pass));

//                // Convierte el arreglo de bytes resultante en una cadena hexadecimal
//                var encryptedStr = "";
//                for (int i = 0; i < result.Length; i++)
//                {
//                    encryptedStr += result[i].ToString("x2").ToLower();
//                }
//                // Asigna la contraseña cifrada al objeto Usuario
//                user.Pass = encryptedStr;
//            }
//        }

//        //METODO QUE COMPARA LA CONTRASEÑA INGRESADA CON LA ALMACENADA DEL USUARIO PARA PERMITIR MODIIFCARLA
//        public static string HashMD5(string password)
//        {
//            // Crea una instancia del algoritmo MD5.
//            using (var md5 = MD5.Create())
//            {
//                // Convierte la cadena de texto (contraseña) a un arreglo de bytes usando la codificación ASCII.
//                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

//                // Convierte el arreglo de bytes resultante en una cadena hexadecimal.
//                var encryptedStr = "";
//                for (int i = 0; i < result.Length; i++)
//                {
//                    encryptedStr += result[i].ToString("x2").ToLower();
//                }
//                // Devuelve la contraseña cifrada en formato de cadena hexadecimal.
//                return encryptedStr;
//            }
//        }


//        // MÉTODO PARA VERIFICAR SI EL NOMBRE DE USUARIO O TELÉFONO YA EXISTE
//        private static async Task<bool> ExistsLogin(Usuario user, ContextoBD context)
//        {
//            bool result = false;

//            // Consulta en la base de datos si ya existe un usuario con el mismo nombre de usuario (correo) pero con un ID diferente.
//            var userLoginExists = await context.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == user.NombreUsuario && u.Id != user.Id);

//            // Consulta en la base de datos si ya existe un usuario con el mismo número de teléfono pero con un ID diferente.
//            var userPhoneExists = await context.Usuario.FirstOrDefaultAsync(u => u.Telefono == user.Telefono && u.Id != user.Id);

//            // Si ya existe un usuario con el mismo nombre de usuario o teléfono, el método devolverá 'true'.
//            if (userLoginExists != null || userPhoneExists != null)
//            {
//                result = true;
//            }

//            return result;
//        }



//        // MÉTODO PARA CREAR UN NUEVO USUARIO 
//        public static async Task<int> Create(Usuario usuario)
//        {
//            int result = 0;
//            using (var dbContext = new ContextoBD())
//            {
//                // Verificar si ya existe un usuario con el mismo correo o teléfono
//                var userLoginExists = await dbContext.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == usuario.NombreUsuario && u.Id != usuario.Id);
//                var userPhoneExists = await dbContext.Usuario.FirstOrDefaultAsync(u => u.Telefono == usuario.Telefono && u.Id != usuario.Id);

//                // Si el correo ya está registrado
//                if (userLoginExists != null)
//                {
//                    throw new Exception("El correo electrónico ya pertenece a un usuario");
//                }

//                // Si el teléfono ya está registrado
//                if (userPhoneExists != null)
//                {
//                    throw new Exception("Este número de teléfono ya está registrado");
//                }

//                // Si no hay duplicados, continúa con la creación del usuario
//                usuario.FechaRegistro = DateTime.Now;
//                EncryptMD5(usuario);

//                if (!string.IsNullOrEmpty(usuario.FotoPerfil))
//                {
//                    usuario.FotoPerfil = usuario.FotoPerfil;
//                }

//                dbContext.Usuario.Add(usuario);
//                result = await dbContext.SaveChangesAsync();
//            }
//            return result;
//        }



//        // MÉTODO PARA ACTUALIZAR UN USUARIO EXISTENTE 
//        public static async Task<int> Update(Usuario usuario)
//        {
//            int result = 0;
//            using (var dbContext = new ContextoBD())
//            {
//                bool existsLogin = await ExistsLogin(usuario, dbContext);
//                if (existsLogin == false)
//                {
//                    var userDb = await dbContext.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
//                    userDb.IdCargo = usuario.IdCargo;
//                    userDb.Nombre = usuario.Nombre;
//                    userDb.Apellido = usuario.Apellido;
//                    userDb.Status = usuario.Status;
//                    userDb.Telefono = usuario.Telefono;
//                    userDb.Pass = usuario.Pass;
//                    userDb.FechaNacimiento = usuario.FechaNacimiento;
//                    userDb.FechaRegistro = usuario.FechaRegistro;
//                    userDb.NombreUsuario = usuario.NombreUsuario;
//                    // Actualiza la foto de perfil solo si no es nula
//                    if (!string.IsNullOrEmpty(usuario.FotoPerfil))
//                    {
//                        userDb.FotoPerfil = usuario.FotoPerfil;
//                    }
//                    // Solo actualiza la contraseña si se ha proporcionado una nueva
//                    if (!string.IsNullOrEmpty(usuario.Pass) && usuario.Pass != userDb.Pass)
//                    {
//                        EncryptMD5(usuario); // Encripta la nueva contraseña
//                        userDb.Pass = usuario.Pass; // Se asigna la contraseña encriptada
//                    }
//                    else
//                    {
//                        // Si no se proporciona una nueva contraseña, se mantiene la existente
//                        userDb.Pass = userDb.Pass; // No se cambia la contraseña
//                    }
//                    dbContext.Usuario.Update(userDb);
//                    result = await dbContext.SaveChangesAsync();
//                }
//                else
//                    throw new Exception("Este correo electrónico ya existe en los registros");
//            }
//            return result;
//        }


//        // MÉTODO PARA ELIMINAR UN USUARIO 
//        public static async Task<int> Delete(Usuario usuario)

//        {
//            // Se verifica si el usuario tiene referencias y se obtiene un mensaje si es así.
//            string mensajeReferencia = await TieneReferencias(usuario.Id);
//            if (mensajeReferencia != null)
//            {
//                throw new Exception(mensajeReferencia); // Lanza una excepción con el mensaje específico.
//            }

//            int result = 0;
//            using (var bdContext = new ContextoBD())
//            {
//                // Busca el usuario existente por su ID.
//                var usuarioDB = await bdContext.Usuario.FirstOrDefaultAsync(u => u.Id == usuario.Id);
//                if (usuarioDB != null)
//                {
//                    bdContext.Usuario.Remove(usuarioDB);
//                    result = await bdContext.SaveChangesAsync();
//                }
//            }
//            // Retorna el resultado (número de filas afectadas en la base de datos).
//            return result;
//        }


//        // MÉTODO PARA OBTENER UN USUARIO POR SU ID 
//        public static async Task<Usuario> GetByIdAsync(Usuario usuario)
//        {
//            var usuarioDB = new Usuario();
//            using (var bdContexto = new ContextoBD())
//            {
//                // Busca el usuario por su ID y asigna el resultado a la variable usuarioDB.
//                usuarioDB = await bdContexto.Usuario.Include(u => u.Cargo).FirstOrDefaultAsync(u => u.Id == usuario.Id);
//            }
//            // Retorna el usuario encontrado.
//            return usuarioDB;
//        }


//        // MÉTODO PARA OBTENER UN USUARIO POR SU NOMBRE DE USUARIO 
//        public static async Task<Usuario> GetByNombreUsuarioAsync(Usuario usuario)
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                // Busca el usuario por su nombre de usuario.
//                var usuarioDB = await dbContext.Usuario
//                    .Include(u => u.Cargo) // Incluir información de Cargo si es necesario
//                    .FirstOrDefaultAsync(u => u.NombreUsuario == usuario.NombreUsuario);
//                return usuarioDB;
//            }
//        }


//        // MÉTODO PARA OBTENER TODOS LOS USUARIOS 
//        public static async Task<List<Usuario>> GetAllAsync()
//        {
//            var usuarios = new List<Usuario>();
//            using (var bdContexto = new ContextoBD())
//            {
//                // Obtiene todos los usuarios y los asigna a la variable usuarios.
//                usuarios = await bdContexto.Usuario.Include(c => c.Cargo).ToListAsync();
//            }
//            // Retorna la lista de usuarios
//            return usuarios;
//        }

//        internal static IQueryable<Usuario> QuerySelect(IQueryable<Usuario> query, Usuario user)
//        {
//            if (user.Id > 0)
//                query = query.Where(u => u.Id == user.Id);

//            if (user.IdCargo > 0)
//                query = query.Where(u => u.IdCargo == user.IdCargo);

//            if (!string.IsNullOrWhiteSpace(user.Nombre))
//                query = query.Where(u => u.Nombre.Contains(user.Nombre));

//            if (!string.IsNullOrWhiteSpace(user.Apellido))
//                query = query.Where(u => u.Apellido.Contains(user.Apellido));

//            if (!string.IsNullOrWhiteSpace(user.NombreUsuario))
//                query = query.Where(u => u.NombreUsuario.Contains(user.NombreUsuario));

//            if (user.Status > 0)
//                query = query.Where(u => u.Status == user.Status);

//            if (user.FechaRegistro.Year > 1000)
//            {
//                DateTime inicialDate = new DateTime(user.FechaRegistro.Year, user.FechaRegistro.Month, user.FechaRegistro.Day, 0, 0, 0);
//                DateTime finalDate = inicialDate.AddDays(1).AddMilliseconds(-1);
//                query = query.Where(u => u.FechaRegistro >= inicialDate && u.FechaRegistro <= finalDate);
//            }

//            query = query.OrderByDescending(u => u.Id).AsQueryable();

//            if (user.Top_Aux > 0)
//                query = query.Take(user.Top_Aux).AsQueryable();

//            return query;
//        }

//        public static async Task<List<Usuario>> SearchAsync(Usuario usuarios)
//        {
//            var users = new List<Usuario>();
//            using (var dbContext = new ContextoBD())
//            {
//                var select = dbContext.Usuario.AsQueryable();
//                select = QuerySelect(select, usuarios).Include(u => u.Cargo);
//                users = await select.ToListAsync();
//            }
//            return users;
//        }


//        public static async Task<List<Usuario>> SearchIncludeRoleAsync(Usuario user, string query, string filter)
//        {
//            var users = new List<Usuario>();
//            using (var dbContext = new ContextoBD())
//            {
//                var select = dbContext.Usuario.AsQueryable();
//                // Lógica para filtrar según el parámetro
//                if (!string.IsNullOrEmpty(query))
//                {
//                    if (filter == "Apellido")
//                    {
//                        select = select.Where(u => u.Apellido.Contains(query));
//                    }
//                    else if (filter == "NombreUsuario")
//                    {
//                        select = select.Where(u => u.NombreUsuario.Contains(query));
//                    }
//                }

//                select = QuerySelect(select, user).Include(u => u.Cargo).AsQueryable();
//                users = await select.ToListAsync();
//            }
//            return users;
//        }

//        public static async Task<Usuario> LoginAsync(Usuario usuarios)
//        {
//            var userDb = new Usuario();
//            using (var dbContext = new ContextoBD())
//            {
//                EncryptMD5(usuarios);
//                userDb = await dbContext.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == usuarios.NombreUsuario &&
//                u.Pass == usuarios.Pass && u.Status == (byte)User_Status.ACTIVO);
//            }
//            return userDb!;
//        }


//        //ESTE METODO ES PARA VERIFICAR SI EL USUARIO TIENE REFERENCIAS EN OTRAS TABLAS
//        public static async Task<string> TieneReferencias(int usuarioId)
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                //// Verifica si el usuario tiene referencias en la tabla InvitacionProyecto
//                //bool tieneInvitaciones = await dbContext.InvitacionProyecto.AnyAsync(ip => ip.IdUsuario == usuarioId);
//                //if (tieneInvitaciones)
//                //{
//                //    return "Este usuario tiene invitaciones pendientes en proyectos.";
//                //}

//                // Verifica si el usuario tiene referencias en la tabla ProyectoUsuario
//                bool tieneProyectos = await dbContext.ProyectoUsuario.AnyAsync(pu => pu.IdUsuario == usuarioId);
//                if (tieneProyectos)
//                {
//                    return "Este usuario está asociado a un proyecto.";
//                }

//                // Si no tiene referencias
//                return null;
//            }
//        }

//        // Método para generar un token de restablecimiento y asignarlo al usuario
//        public static async Task<int> GenerarCodigoRestablecimientoAsync(Usuario usuario)
//        {
//            // Generar un código de 6 dígitos aleatorio
//            Random random = new Random();
//            int codigo = random.Next(100000, 999999);

//            // Generar el hash MD5 del código
//            string codigoEncriptado = HashMD5(codigo.ToString());

//            var passwordResetCode = new PasswordResetCode
//            {
//                IdUsuario = usuario.Id,
//                Codigo = codigoEncriptado,
//                Expiration = DateTime.Now.AddMinutes(15) // Expira en 15 minutos
//            };

//            using (var dbContext = new ContextoBD())
//            {
//                dbContext.PasswordResetCode.Add(passwordResetCode);
//                await dbContext.SaveChangesAsync();
//            }
//            return codigo;
//        }

//        // Método para verificar si el token de restablecimiento es válido
//        public static async Task<bool> ValidarCodigoRestablecimientoAsync(int Idusuario, string codigo)
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                // Generar el hash del código ingresado
//                string codigoIngresadoEncriptado = HashMD5(codigo.ToString());

//                var resetCode = await dbContext.PasswordResetCode
//                    .FirstOrDefaultAsync(c => c.IdUsuario == Idusuario && c.Codigo == codigoIngresadoEncriptado);

//                // Si el token existe y no ha expirado
//                if (resetCode != null && resetCode.Expiration >= DateTime.Now)
//                {
//                    return true;
//                }
//                return false;
//            }
//        }

//        // Método para restablecer la contraseña del usuario
//        public static async Task<int> RestablecerContrasenaAsync(int Idusuario, string codigo, string nuevaContrasena)
//        {
//            using (var dbContext = new ContextoBD())
//            {
//                string codigoIngresadoEncriptado = HashMD5(codigo.ToString());

//                var resetCode = await dbContext.PasswordResetCode
//                    .FirstOrDefaultAsync(c => c.IdUsuario == Idusuario && c.Codigo == codigoIngresadoEncriptado);

//                if (resetCode != null && resetCode.Expiration >= DateTime.Now)
//                {
//                    var usuario = await dbContext.Usuario.FindAsync(Idusuario);

//                    // Encripta la nueva contraseña y actualiza
//                    usuario.Pass = HashMD5(nuevaContrasena);

//                    // Elimina el token después de restablecer la contraseña
//                    dbContext.PasswordResetCode.Remove(resetCode);

//                    dbContext.Usuario.Update(usuario);
//                    return await dbContext.SaveChangesAsync();
//                }
//                else
//                {
//                    throw new Exception("Token inválido o expirado.");
//                }
//            }
//        }
//    }

//}