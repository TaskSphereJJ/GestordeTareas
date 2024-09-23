﻿

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

        public async Task<int> ChangePasswordAsync(Usuario usuario, string oldPassword)
        {
            return await UsuarioDAL.ChangePasswordAsync(usuario, oldPassword);
        }

    }
}