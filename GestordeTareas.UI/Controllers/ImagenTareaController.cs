using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    public class ImagenTareaController : Controller
    {
        private readonly ImagenTareaBL _imagenTareaBL;

        public ImagenTareaController()
        {
            _imagenTareaBL = new ImagenTareaBL();   
        }

        // GET: ImagenTareaController
        public async Task <ActionResult> Index()
        {
            var imagenTareas = await _imagenTareaBL.GetAllAsync();
            return View(imagenTareas);
        }

        // GET: ImagenTareaController/Details/5
        public async Task <ActionResult> Details(int id)
        {
            var imagenTarea = await _imagenTareaBL.GetById(new ImagenTarea { Id = id });
            return View(imagenTarea);
        }

        // GET: ImagenTareaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImagenTareaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create(ImagenTarea imagenTarea)
        {
            try
            {
                await _imagenTareaBL.CreateAsync(imagenTarea);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ImagenTareaController/Edit/5
        public async Task <ActionResult> Edit(int id)
        {
            var imagenTarea = await _imagenTareaBL.GetById(new ImagenTarea { Id = id });
            return View(imagenTarea);
        }

        // POST: ImagenTareaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ImagenTarea imagenTarea)
        {
            try
            {
                await _imagenTareaBL.UpdateAsync(imagenTarea);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ImagenTareaController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var imagenTarea = await _imagenTareaBL.GetById(new ImagenTarea { Id = id });
            return View(imagenTarea);
        }

        // POST: ImagenTareaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ImagenTarea imagenTarea)
        {
            try
            {
                await _imagenTareaBL.DeleteAsync(imagenTarea);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
