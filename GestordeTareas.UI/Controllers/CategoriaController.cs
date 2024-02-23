using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaBL _categoriaBL;

        public CategoriaController()
        {
            _categoriaBL = new CategoriaBL(); // Inicializamos la capa de negocio
        }

        // GET: CategoriaController
        public async Task<ActionResult> Index()
        {
            var categorias = await _categoriaBL.GetAllAsync();
            return View(categorias);
        }

        // GET: CategoriaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var categoria = await _categoriaBL.GetById(new Categoria { Id = id });
            return View(categoria);
        }

        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Categoria categoria)
        {
            try
            {
                int result = await _categoriaBL.CreateAsync(categoria);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: CategoriaController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var categoria = await _categoriaBL.GetById(new Categoria { Id = id });
            return View(categoria);
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Categoria categoria)
        {
            try
            {
                int result = await _categoriaBL.UpdateAsync(categoria);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(categoria);
            }
        }

        // GET: CategoriaController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _categoriaBL.GetById(new Categoria { Id = id });
            return View(categoria);

        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Categoria categoria)
        {
            try
            {
                await _categoriaBL.DeleteAsync(categoria);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(categoria);
            }
        }
    }
}
