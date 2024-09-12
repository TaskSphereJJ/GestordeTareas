using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CargoController : Controller
    {
        private readonly CargoBL _cargoBL;

        public CargoController()
        {
            _cargoBL = new CargoBL(); // Inicializamos la capa de negocio
        }

        // GET: CargoController
        public async Task<ActionResult> Index()
        {
            List<Cargo> Lista = await _cargoBL.GetAllAsync();

            return View(Lista);
        }

        // GET: CargoController/Details/5
        public async Task<ActionResult> DetailsPartial(int id)
        {
            var cargo = await _cargoBL.GetById(new Cargo { Id = id });
            return PartialView("Details", cargo);
        }

        // GET: CargoController/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: CargoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cargo cargo)
        {
            try
            {
                int result = await _cargoBL.CreateAsync(cargo);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return PartialView("Create", cargo);
            }
        }

        // GET: CargoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var cargo = await _cargoBL.GetById(new Cargo { Id = id });
            return PartialView("Edit", cargo);
        }

        // POST: CargoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Cargo cargo)
        {
            try
            {
                int result = await _cargoBL.UpdateAsync(cargo);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(cargo);
            }
        }

        // GET: CargoController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var cargo = await _cargoBL.GetById(new Cargo { Id = id });
            return PartialView("Delete", cargo);

        }

        // POST: CargoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Cargo cargo)
        {
            try
            {
                await _cargoBL.DeleteAsync(cargo);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(cargo);
            }
        }
    }
}
