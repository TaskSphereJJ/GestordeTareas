using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
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
        public async Task<ActionResult> Details(int id)
        {
            var prioridad = await _prioridadBL.GetById(new Prioridad { Id = id });
            return View(prioridad);
        }

        // GET: PrioridadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrioridadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Prioridad prioridad)
        {
            try
            {
                int result = await _prioridadBL.CreateAsync(prioridad);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: PrioridadController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var prioridad = await _prioridadBL.GetById(new Prioridad { Id = id });
            return View(prioridad);
        }

        // POST: PrioridadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Prioridad prioridad)
        {
            try
            {
                int result = await _prioridadBL.UpdateAsync(prioridad);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(prioridad);
            }
        }

        // GET: PrioridadController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var prioridad = await _prioridadBL.GetById(new Prioridad { Id = id });
            return View(prioridad);

        }

        // POST: PrioridadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Prioridad prioridad)
        {
            try
            {
                await _prioridadBL.DeleteAsync(prioridad);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(prioridad);
            }
        }
    }
}
