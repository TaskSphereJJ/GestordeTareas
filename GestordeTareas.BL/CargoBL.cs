using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class CargoBL
    {
        public async Task<int> CreateAsync(Cargo cargo)
        {
            return await CargoDAL.CreateAsync(cargo);
        }
        public async Task<int> UpdateAsync(Cargo cargo)
        {
            return await CargoDAL.UpdateAsync(cargo);
        }
        public async Task<int> DeleteAsync(Cargo cargo)
        {
            return await CargoDAL.DeleteAsync(cargo);
        }
        public async Task<Cargo> GetById(Cargo cargo)
        {
            return await CargoDAL.GetByIdAsync(cargo);
        }
        public async Task<List<Cargo>> GetAllAsync()
        {
            return await CargoDAL.GetAllAsync();
        }
    }
}
