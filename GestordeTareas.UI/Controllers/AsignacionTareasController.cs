using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    public class AsignacionTareasController : Controller
    {
        private readonly AsignacionTareasBL _asignacionTareasBL;

        public AsignacionTareasController()
        {
            _asignacionTareasBL = new AsignacionTareasBL();
        }

        // GET: AsignacionTareasController
        public async Task<ActionResult> Index()
        {
            var asignacionTarea = await _asignacionTareasBL.GetAllAsync();
            return View(asignacionTarea);
        }

        // GET: AsignacionTareasController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var asignacionTarea = await _asignacionTareasBL.GetById(new AsignacionTareas { Id = id });
            return View(asignacionTarea);
        }

        // GET: AsignacionTareasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AsignacionTareasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AsignacionTareas asignacionTareas)
        {
            try
            {
                await _asignacionTareasBL.CreateAsync(asignacionTareas);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AsignacionTareasController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var categoria = await _asignacionTareasBL.GetById(new AsignacionTareas { Id = id });
            return View(categoria);
        }

        // POST: AsignacionTareasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AsignacionTareas asignacionTareas)
        {
            try
            {
                await _asignacionTareasBL.UpdateAsync(asignacionTareas);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AsignacionTareasController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var asignacionTarea = await _asignacionTareasBL.GetById(new AsignacionTareas { Id = id });
            return View(asignacionTarea);
        }

        public async Task<ActionResult> Delete(int id, AsignacionTareas asignacionTareas)
        {
            try
            {
                await _asignacionTareasBL.DeleteAsync(asignacionTareas);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }

        
    
}
