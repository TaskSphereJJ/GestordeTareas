

using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class UsuarioBL
    {
        public async Task<int> Create(Usuario usuario)
        {
            return await UsuarioDAL.Create(usuario);
        }

        public async Task<int> Update(Usuario usuario)
        {
            return await UsuarioDAL.Update(usuario);
        }

        public async Task<int> Delete(Usuario usuario)
        {
            return await UsuarioDAL.Delete(usuario);
        }

        public async Task<Usuario> GetByIdAsync(Usuario usuario)
        {
            return await UsuarioDAL.GetByIdAsync(usuario);
        }

        // Método para obtener un usuario por su nombre de usuario
        public async Task<Usuario> GetByNombreUsuarioAsync(Usuario usuario)
        {
            return await UsuarioDAL.GetByNombreUsuarioAsync(usuario);
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await UsuarioDAL.GetAllAsync();
        }

        public async Task<List<Usuario>> SearchAsync(Usuario usuarios)
        {
            return await UsuarioDAL.SearchAsync(usuarios);
        }

        public async Task<List<Usuario>> SearchIncludeRoleAsync(Usuario user)
        {
            return await UsuarioDAL.SearchIncludeRoleAsync(user);
        }

        public async Task<Usuario> LoginAsync(Usuario usuarios)
        {
            return await UsuarioDAL.LoginAsync(usuarios);
        }

        // Metoro para registrar un usuario autenticado con Google
        public async Task<int> RegisterGoogleUserAsync(Usuario usuario)
        {
            // Validación básica de los datos del usuario proporcionados por Google
            if (string.IsNullOrEmpty(usuario.NombreUsuario))
                throw new ArgumentException("El nombre de usuario es requerido");

            if (string.IsNullOrEmpty(usuario.Provider) || string.IsNullOrEmpty(usuario.ProviderKey))
                throw new ArgumentException("La autenticación del proveedor es inválida");

            // Delegamos la creación del usuario a la capa DAL
            return await GestordeTareas.DAL.UsuarioDAL.CreateGoogleUserAsync(usuario);
        }
        public async Task<Usuario?> LoginGoogleUserAsync(string providerKey, string provider)
        {
            // Validamos que los valores requeridos no estén vacíos
            if (string.IsNullOrEmpty(providerKey))
                throw new ArgumentException("El identificador del proveedor es requerido");

            if (string.IsNullOrEmpty(provider))
                throw new ArgumentException("El proveedor es requerido");

            // llamo al método de la DAL para obtener el usuario
            return await GestordeTareas.DAL.UsuarioDAL.LoginGoogleUserAsync(providerKey, provider);
        }



    }
}