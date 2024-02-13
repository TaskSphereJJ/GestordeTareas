using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class CargoDAL
    {
        //--------------------------------METODO CREAR CARGO.--------------------------
        public static async Task<int> CreateAsync(Cargo cargo)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD()) //el comando using hace un proceso de ejecucion
            {
                dbContexto.Cargo.Add(cargo); //agrego un nuevo categorua
                result = await dbContexto.SaveChangesAsync();//se guarda a la base de datos
            }
            return result;
        }
        //--------------------------------METODO MODIFICAR cargo.--------------------------
        public static async Task<int> UpdateAsync(Cargo cargo)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())//hago una instancia de la base de datos
            {
                //expresion landam
                var cargoBD = await bdContexto.Categoria.FirstOrDefaultAsync(c => c.Id == cargo.Id); //lo busco 
                if (cargoBD != null)//verifico que no este nulo
                {
                    cargoBD.Nombre = cargo.Nombre; //actualizo las propiedades
                    bdContexto.Update(cargoBD); //se guarda en memora
                    result = await bdContexto.SaveChangesAsync(); //guardo en la base de datos con SaveChangesAsync
                }
            }
            return result;
        }
        //--------------------------------METODO MODIFICAR CATEGORIA.--------------------------
    }
}
