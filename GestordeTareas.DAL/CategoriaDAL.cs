using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.DAL
{      
    public class CategoriaDAL
    {
        //--------------------------------METODO CREAR CATEGORIA.--------------------------
        public static async Task<int> CreateAsync(Categoria categoria)
        {
            int result = 0;
            using (var dbContexto = new ContextoBD()) //el comando using hace un proceso de ejecucion
            {
                dbContexto.Categoria.Add(categoria); //agrego un nuevo categorua
                result = await dbContexto.SaveChangesAsync();//se guarda a la base de datos
            }
            return result;
        }
        //--------------------------------METODO MODIFICAR CATEGORIA.--------------------------
        public static async Task<int> UpdateAsync(Categoria categoria)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD())//hago una instancia de la base de datos
            {
                //expresion landam
                var categoriaBD = await bdContexto.Categoria.FirstOrDefaultAsync(c => c.Id == categoria.Id); //lo busco 
                if (categoriaBD != null)//verifico que no este nulo
                {
                    categoriaBD.Nombre = categoria.Nombre; //actualizo las propiedades
                    bdContexto.Update(categoriaBD); //se guarda en memora
                    result = await bdContexto.SaveChangesAsync(); //guardo en la base de datos con SaveChangesAsync
                }
            }
            return result;
        }
        //--------------------------------METODO Eliminar CATEGORIA.--------------------------
        public static async Task<int> DeleteAsync(Categoria categoria)
        {
            int result = 0;
            using (var bdContexto = new ContextoBD()) //istancio la coneccion
            {
                var categoriaBD = await bdContexto.Categoria.FirstOrDefaultAsync(c => c.Id == categoria.Id); //busco el id
                if (categoriaBD != null)//verifico que no este nulo
                {
                    bdContexto.Categoria.Remove(categoriaBD);//elimino anivel de memoria la categoria
                    result = await bdContexto.SaveChangesAsync();//le digo a la BD que se elimine y se guarde
                }
            }
            return result;
        }
        //--------------------------------METODO obtenerporID CATEGORIA.--------------------------
        public static async Task<Categoria> GetByIdAsync(Categoria categoria)
        {
            var categoryBD = new Categoria();
            using (var bdContexto = new ContextoBD())
            {
                categoryBD = await bdContexto.Categoria.FirstOrDefaultAsync(c => c.Id == categoria.Id); //busco el id
            }
            return categoryBD;
        }

        //--------------------------------METODO obtener todas las CATEGORIAS.--------------------------
        public static async Task<List<Categoria>> GetAllAsync()
        {
            var categorias = new List<Categoria>(); //una variable de lo que llevara una lista de Categorias
            using (var bdContexto = new ContextoBD()) //creo el acceso a la BD
            {
                categorias = await bdContexto.Categoria.ToListAsync(); //le digo que categories contenga la lista de categorias, osea lo de l BD
            }
            return categorias;
        }
         internal static IQueryable<Categoria> QuerySelect(IQueryable<Categoria> query, Categoria category)
        {
            if(category.Id > 0)
                query = query.Where(c => c.Id == category.Id);

            if(!string.IsNullOrWhiteSpace(category.Nombre))
                query = query.Where(c => c.Nombre.Contains(category.Nombre));

            query = query.OrderByDescending(c => c.Id);

            if (category.Top_Aux > 0)
                query = query.Take(category.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<Categoria>> SearchAsync(Categoria category)
        {
            var categories = new List<Categoria>();
            using(var dbContext = new ContextoBD())
            {
                var select = dbContext.Categoria.AsQueryable();
                select = QuerySelect(select, category);
                categories = await select.ToListAsync();
            }
            return categories;
        }
    }
}