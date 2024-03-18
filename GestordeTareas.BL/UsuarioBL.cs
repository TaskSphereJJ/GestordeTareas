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
        public async Task<int> CreateAsync(Usuario usuario)
        {
            return await UsuarioDAL.CreateAsync(usuario);
        }
        public async Task<int> UpdateAsync(Usuario usuario)
        {
            return await UsuarioDAL.UpdateAsync(usuario);
        }
        public async Task<int> DeleteAsync(Usuario usuario)
        {
            return await UsuarioDAL.DeleteAsync(usuario);
        }

        public async Task<Usuario> GetById(Usuario usuario)
        {
            return await UsuarioDAL.GetByIdAsync(usuario);
        }
        public async Task<List<Usuario>> GetAllAsync()
        {
            return await UsuarioDAL.GetAllAsync();
        }

    }
}
