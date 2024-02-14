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
        //--------------------------------METODO ELIMINAR CARGO.--------------------------
        public static async Task<int> DeleteAsync(Cargo cargo)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD()) //istancio la coneccion
            {
                var cargoBD = await bdContexto.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id); //busco el id
                if (cargoBD != null)//verifico que no este nulo
                {
                    bdContexto.Cargo.Remove(cargoBD);//elimino anivel de memoria la categoria
                    result = await bdContexto.SaveChangesAsync();//le digo a la BD que se elimine y se guarde
                }
            }
            return result;
        }
        //--------------------------------METODO obtenerporID CATEGORIA.--------------------------
        public static async Task<Cargo> GetByIdAsync(Cargo cargo)
        {
            var cargoBD = new Cargo();
            using (var bdContexto = new ContextoBD())
            {
                var cargob = await bdContexto.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id); //busco el id
            }
            return cargoBD;
        }
        //--------------------------------METODO obtener todas las CATEGORIAS.--------------------------
        public static async Task<List<Cargo>> GetAllAsync()
        {
            var cargos = new List<Cargo>(); //una variable de lo que llevara una lista de Categorias
            using (var bdContexto = new ContextoBD()) //creo el acceso a la BD
            {
                cargos = await bdContexto.Cargo.ToListAsync(); //le digo que categories contenga la lista de categorias, osea lo de l BD
            }
            return cargos;
        }
    }
}
