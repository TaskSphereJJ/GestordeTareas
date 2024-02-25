using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class IniciarSesionDAL
    {
        private readonly ContextoBD _context;

        public IniciarSesionDAL(ContextoBD context)
        {
            _context = context;
        }

        // Método para registrar un nuevo usuario de forma asincrónica.
        public async Task<int> RegistrarUsuarioAsync(IniciarSesionEN usuario)
        {
            _context.IniciarSesion.Add(usuario);
            return await _context.SaveChangesAsync();
        }

        // Método para buscar un usuario por nombre usuario
        public async Task<IniciarSesionEN> GetNombreUsuario(IniciarSesionEN nombreUsuario)
        {
            return await _context.IniciarSesion.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario.NombreUsuario);
        }

        // Método para verificar las credenciales de inicio de sesión de un usuario por correo y contraseña.
        public async Task<bool> VerificarCredencialesAsync(string nombreUsuario, string contrasena)
        {
            var usuario = await _context.IniciarSesion.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Pass == contrasena);
            return usuario != null;
        }
    }
}
