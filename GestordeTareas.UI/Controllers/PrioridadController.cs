using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(Roles = "Administrador", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PrioridadController : Controller
    {

        private readonly PrioridadBL _prioridadBL;

        public PrioridadController()
        {
            _prioridadBL = new PrioridadBL(); // Inicializamos la capa de negocio
        }

        // GET: PrioridadController
        public async Task<ActionResult> Index()
        {
            List<Prioridad> Lista = await _prioridadBL.GetAllAsync();
            return View(Lista);
        }

        // GET: PrioridadController/Details/5
        public async Task<ActionResult> DetailsPartial(int id)
        {
            var prioridad = await _prioridadBL.GetById(new Prioridad { Id = id });
            return PartialView("Details", prioridad);
        }

        // GET: PrioridadController/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: PrioridadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Prioridad prioridad)
        {
            try
            {
                int result = await _prioridadBL.CreateAsync(prioridad);
                return Json(new { success = true, message = "Prioridad creada correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al crear la prioridad: {ex.Message}" });
            }
        }

        // GET: PrioridadController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var prioridad = await _prioridadBL.GetById(new Prioridad { Id = id });
            return PartialView("Edit", prioridad);
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Prioridad prioridad)
        {
            try
            {
                int result = await _prioridadBL.UpdateAsync(prioridad);
                return Json(new { success = true, message = "Prioridad editada correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al editar la prioridad: {ex.Message}" });
            }
        }

        // GET: PrioridadController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var prioridad = await _prioridadBL.GetById(new Prioridad { Id = id });
            return PartialView("Delete", prioridad);

        }

        // POST: PrioridadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Prioridad prioridad)
        {
            try
            {
                await _prioridadBL.DeleteAsync(prioridad);
                return Json(new { success = true, message = "Prioridad eliminada correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al eliminar la prioridad: {ex.Message}" });
            }
        }
    }
}
