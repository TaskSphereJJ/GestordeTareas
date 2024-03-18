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
        public async Task<int> CreateAsync(Usuario usuarios)
        {
            return await UsuarioDAL.CreateAsync(usuarios);
        }
        public async Task<int> UpdateAsync(Usuario usuarios)
        {
            return await UsuarioDAL.UpdateAsync(usuarios);
        }
        public async Task<int> DeleteAsync(Usuario usuarios)
        {
            return await UsuarioDAL.DeleteAsync(usuarios);
        }

        public async Task<Usuario> GetById(Usuario usuarios)
        {
            return await UsuarioDAL.GetByIdAsync(usuarios);
        }
        public async Task<List<Usuario>> GetAllAsync()
        {
            return await UsuarioDAL.GetAllAsync();
        }

    }
}
