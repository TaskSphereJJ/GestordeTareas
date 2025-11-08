using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{
    public class CargoDAL
    {
        // Crear un nuevo Cargo
        public static async Task<int> CreateAsync(Cargo cargo)
        {
            using var dbContext = new ContextoBD();
            dbContext.Cargo.Add(cargo);
            return await dbContext.SaveChangesAsync();
        }

        // Actualizar un Cargo existente
        public static async Task<int> UpdateAsync(Cargo cargo)
        {
            using var dbContext = new ContextoBD();
            var cargoBD = await dbContext.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id);

            if (cargoBD == null) return 0;

            cargoBD.Nombre = cargo.Nombre;
            dbContext.Update(cargoBD);
            return await dbContext.SaveChangesAsync();
        }

        // Eliminar un Cargo (verifica referencias)
        public static async Task<int> DeleteAsync(Cargo cargo)
        {
            using var dbContext = new ContextoBD();
            var cargoBD = await dbContext.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id);

            if (cargoBD == null) return 0;

            bool isAssociatedWithUsuario = await dbContext.Usuario.AnyAsync(u => u.IdCargo == cargoBD.Id);
            if (isAssociatedWithUsuario)
                throw new Exception("No se puede eliminar el cargo porque está asociado con un usuario.");

            dbContext.Cargo.Remove(cargoBD);
            return await dbContext.SaveChangesAsync();
        }

        // Obtener un Cargo por ID
        public static async Task<Cargo> GetByIdAsync(Cargo cargo)
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id);
        }

        // Obtener todos los Cargos
        public static async Task<List<Cargo>> GetAllAsync()
        {
            using var dbContext = new ContextoBD();
            return await dbContext.Cargo.ToListAsync();
        }

        // Obtener el ID del cargo "Colaborador"
        public static async Task<int> GetCargoColaboradorIdAsync()
        {
            using var dbContext = new ContextoBD();
            var cargoColaborador = await dbContext.Cargo.FirstOrDefaultAsync(c => c.Nombre == "Colaborador");
            return cargoColaborador?.Id ?? 0;
        }
    }
}
























//using GestordeTaras.EN;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GestordeTareas.DAL
//{
//    public class CargoDAL
//    {
//        //--------------------------------METODO CREAR CARGO.--------------------------
//        public static async Task<int> CreateAsync(Cargo cargo)
//        {
//            int result = 0;
//            using (var dbContexto = new ContextoBD()) //el comando using hace un proceso de ejecucion
//            {
//                dbContexto.Cargo.Add(cargo); //agrego un nuevo categorua
//                result = await dbContexto.SaveChangesAsync();//se guarda a la base de datos
//            }
//            return result;
//        }
//        //--------------------------------METODO MODIFICAR cargo.--------------------------
//        public static async Task<int> UpdateAsync(Cargo cargo)
//        {
//            int result = 0;
//            using (var bdContexto = new ContextoBD())//hago una instancia de la base de datos
//            {
//                //expresion landam
//                var cargoBD = await bdContexto.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id); //lo busco 
//                if (cargoBD != null)//verifico que no este nulo
//                {
//                    cargoBD.Nombre = cargo.Nombre; //actualizo las propiedades
//                    bdContexto.Update(cargoBD); //se guarda en memora
//                    result = await bdContexto.SaveChangesAsync(); //guardo en la base de datos con SaveChangesAsync
//                }
//            }
//            return result;
//        }
//        //--------------------------------METODO ELIMINAR CARGO.--------------------------
//        public static async Task<int> DeleteAsync(Cargo cargo)
//        {
//            int result = 0;
//            using (var bdContexto = new ContextoBD()) //istancio la coneccion
//            {
//                var cargoBD = await bdContexto.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id); //busco el id
//                if (cargoBD != null)//verifico que no este nulo
//                {
//                    // Verificar si la categoría está asociada con alguna tarea
//                    bool isAssociatedWithUsuario = await bdContexto.Usuario.AnyAsync(t => t.IdCargo == cargoBD.Id);
//                    if (isAssociatedWithUsuario)
//                    {
//                        // Si está asociada, lanzar una excepción
//                        throw new Exception("No se puede eliminar el cargo porque está asociado con un usuario.");
//                    }
//                    bdContexto.Cargo.Remove(cargoBD);//elimino anivel de memoria la categoria
//                    result = await bdContexto.SaveChangesAsync();//le digo a la BD que se elimine y se guarde
//                }
//            }
//            return result;
//        }
//        //--------------------------------METODO obtenerporID CARGO.--------------------------
//        public static async Task<Cargo> GetByIdAsync(Cargo cargo)
//        {
//            var cargoBD = new Cargo();
//            using (var bdContexto = new ContextoBD())
//            {
//                cargoBD = await bdContexto.Cargo.FirstOrDefaultAsync(c => c.Id == cargo.Id); //busco el id y asigno el resultado a cargoBD
//            }
//            return cargoBD;
//        }

//        //--------------------------------METODO obtener todas las CATEGORIAS.--------------------------
//        public static async Task<List<Cargo>> GetAllAsync()
//        {
//            var cargos = new List<Cargo>(); //una variable de lo que llevara una lista de Categorias
//            using (var bdContexto = new ContextoBD()) //creo el acceso a la BD
//            {
//                cargos = await bdContexto.Cargo.ToListAsync(); //le digo que categories contenga la lista de categorias, osea lo de l BD
//            }
//            return cargos;
//        }

//        public static async Task<int> GetCargoColaboradorIdAsync()
//        {
//            int cargoColaboradorId = 0;
//            using (var dbContext = new ContextoBD())
//            {
//                // Busca el cargo "Colaborador" en la base de datos y obtén su ID.
//                var cargoColaborador = await dbContext.Cargo.FirstOrDefaultAsync(c => c.Nombre == "Colaborador");
//                if (cargoColaborador != null)
//                {
//                    cargoColaboradorId = cargoColaborador.Id;
//                }
//            }
//            // Retorna el ID del cargo "Colaborador".
//            return cargoColaboradorId;
//        }
//    }
//}
