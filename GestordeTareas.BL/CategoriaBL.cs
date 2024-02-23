using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class CategoriaBL
    {
        public async Task<int> CreateAsync(Categoria categoria)
        {
            return await CategoriaDAL.CreateAsync(categoria);
        }
        public async Task<int> UpdateAsync(Categoria categoria)
        {
            return await CategoriaDAL.UpdateAsync(categoria);
        }
        public async Task<int> DeleteAsync(Categoria categoria)
        {
            return await CategoriaDAL.DeleteAsync(categoria);
        }

        public async Task<Categoria> GetById(Categoria categoria)
        {
            return await CategoriaDAL.GetByIdAsync(categoria);
        }
        public async Task<List<Categoria>> GetAllAsync()
        {
            return await CategoriaDAL.GetAllAsync();
        }
    }
}
