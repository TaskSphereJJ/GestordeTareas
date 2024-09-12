using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
            List<Categoria> Lista = await _categoriaBL.GetAllAsync();

            return View(Lista);
        }


        // GET: CategoriaController/Details/5
        public async Task<ActionResult> DetailsPartial(int id)
        {
            var categoria = await _categoriaBL.GetById(new Categoria { Id = id });
            return PartialView("Details", categoria);
        }

        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return PartialView("Create");
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
                return PartialView("Create", categoria);
            }
        }

        // GET: CategoriaController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var categoria = await _categoriaBL.GetById(new Categoria { Id = id });
            return PartialView("Edit", categoria);
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
            return PartialView("Delete", categoria);

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