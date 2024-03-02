using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestordeTareas.UI.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaBL _categoriaBL;

        public CategoriaController()
        {
            _categoriaBL = new CategoriaBL(); // Inicializamos la capa de negocio
        }

        // GET: Categoria/Index
        public async Task<IActionResult> Index()
        {
            List<Categoria> Lista = await _categoriaBL.GetAllAsync();
            return View(Lista);
        }

        [HttpPost]
        public async Task<IActionResult> Create (Categoria Categoria)
        {
            try { 
            //Logica para crear una nueva categoria
            await _categoriaBL.CreateAsync(Categoria);
            return Json(new { Success = true });
        }
        catch (Exception ex)
            {
                return Json(new {success = false, error = ex.Message});
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            try
            {
                // Lógica para actualizar la categoría existente
                categoria.Id = id; 
                await _categoriaBL.UpdateAsync(categoria);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Lógica para eliminar la categoría
                var categoria = await _categoriaBL.GetById(new Categoria { Id = id });
                if (categoria != null)
                {
                    await _categoriaBL.DeleteAsync(categoria);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = "La categoría no existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // Lógica para obtener los detalles de la categoría
                var categoria = await _categoriaBL.GetById(new Categoria { Id = id });
                return View(categoria); 
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}



