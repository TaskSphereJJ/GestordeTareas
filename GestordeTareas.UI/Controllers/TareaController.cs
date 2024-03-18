using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    public class TareaController : Controller
    {
        private readonly TareaBL _tareaBL;

        public TareaController()
        {
            _tareaBL = new TareaBL(); // Inicializamos la capa de negocio
        }

        // GET: TareaController
        public async Task<ActionResult> Index()
        {
            List<Tarea> Lista = await _tareaBL.GetAllAsync();

            return View(Lista);
        }

        // GET: TareaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            return PartialView("Details", tarea);
        }

        // GET: TareaController/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: TareaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tarea tarea)
        {
           try
            {
                int result = await _tareaBL.CreateAsync(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return PartialView("Create", tarea);
            }
        }

        // GET: TareaController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            return PartialView("Edit", tarea);
        }

        // POST: TareaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Tarea tarea)
        {
            try
            {
                int result = await _tareaBL.UpdateAsync(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(tarea);
            }
        }


        // GET: TareaController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            return PartialView("Delete", tarea);
        }

        // POST: TareaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Tarea tarea)
        {
            try
            {
                await _tareaBL.DeleteAsync(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(tarea);
            }
        }
    }
}
