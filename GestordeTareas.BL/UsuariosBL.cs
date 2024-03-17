using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class UsuariosBL
    {
        public async Task<int> Create(Usuarios usuario)
        {
            return await UsuarioDAL.Create(usuario);
        }

        public async Task<int> Update(Usuarios usuario)
        {
            return await UsuarioDAL.Update(usuario);
        }

        public async Task<int> Delete(Usuarios usuario)
        {
            return await UsuarioDAL.Delete(usuario);
        }

        public async Task<Usuarios> GetByIdAsync(Usuarios usuario)
        {
            return await UsuarioDAL.GetByIdAsync(usuario);
        }

        public async Task<List<Usuarios>> GetAllAsync()
        {
            return await UsuarioDAL.GetAllAsync();
        }

        public async Task<List<Usuarios>> SearchAsync(Usuarios usuarios)
        {
            return await UsuarioDAL.SearchAsync(usuarios);
        }

        public async Task<List<Usuarios>> SearchIncludeRoleAsync(Usuarios user)
        {
            return await UsuarioDAL.SearchIncludeRoleAsync(user);
        }

        public async Task<Usuarios> LoginAsync(Usuarios usuarios)
        {
            return await UsuarioDAL.LoginAsync(usuarios);
        }

        public async Task<int> ChangePasswordAsync(Usuarios usuario, string oldPassword)
        {
            return await UsuarioDAL.ChangePasswordAsync(usuario, oldPassword);
        }

       
    }
}
