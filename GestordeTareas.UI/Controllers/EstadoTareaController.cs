using GestordeTareas.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestordeTaras.EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace GestordeTareas.UI.Controllers
{

    [Authorize(Roles = "Administrador", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class EstadoTareaController : Controller

    {
        private readonly EstadoTareaBL _estadoTareaBL;

        public EstadoTareaController()
        {
            _estadoTareaBL = new EstadoTareaBL(); // Inicializamos la capa de negocio
        }

        // GET: CategoriaController
        public async Task<ActionResult> Index()
        {
            List<EstadoTarea> Lista = await _estadoTareaBL.GetAllAsync();

            return View(Lista);
        }

        // GET: CategoriaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var estadoTarea = await _estadoTareaBL.GetByIdAsync(new EstadoTarea { Id = id });
            return PartialView("Details", estadoTarea);
        }

        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EstadoTarea estadoTarea)
        {
            try
            {
                int result = await _estadoTareaBL.CreateAsync(estadoTarea);
                return Json(new { success = true, message = "Estado creado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al crear el estado: {ex.Message}" });
            }
        }

        // GET: CategoriaController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var estadoTarea = await _estadoTareaBL.GetByIdAsync(new EstadoTarea   { Id = id });
            return PartialView("Edit", estadoTarea);
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EstadoTarea estadoTarea)
        {
            try
            {
                int result = await _estadoTareaBL.UpdateAsync(estadoTarea);
                return Json(new { success = true, message = "Estado editado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al editar el estado: {ex.Message}" });
            }
        }

        // GET: CategoriaController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var estadoTarea = await _estadoTareaBL.GetByIdAsync(new EstadoTarea { Id = id });
            return PartialView("Delete", estadoTarea);

        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, EstadoTarea estadoTarea)
        {
            try
            {
                await _estadoTareaBL.DeleteAsync(estadoTarea);
                return Json(new { success = true, message = "Estado eliminado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al eliminar el estado: {ex.Message}" });
            }
        }
    }
}
