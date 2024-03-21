using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GestordeTareas.UI.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly ProyectoBL _proyectoBL;
        private readonly UsuarioBL _usuarioBL;

        public ProyectoController()
        {
            _proyectoBL = new ProyectoBL();
            _usuarioBL = new UsuarioBL();
        }

        // GET: ProyectoController
        public async Task<ActionResult> Index()
        {
            List<Proyecto> Lista = await _proyectoBL.GetAllAsync();

            return View(Lista);
        }


        // GET: ProyectoController/Details/5
        public async Task<ActionResult> DetailsPartial(int id)
        {
            var proyecto = await _proyectoBL.GetById(new Proyecto { Id = id });
            return PartialView("Details", proyecto);
        }

        // GET: ProyectoController/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: ProyectoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Proyecto proyecto)
        {
            try
            {
                // Obtener el IdUsuario del usuario autenticado
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
                var actualUser = users.FirstOrDefault();

                // Asignar el IdUsuario al proyecto
                proyecto.IdUsuario = actualUser.Id;

                int result = await _proyectoBL.CreateAsync(proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return PartialView("Create", proyecto);
            }
        }


        // GET: ProyectoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var proyecto = await _proyectoBL.GetById(new Proyecto { Id = id });
            return PartialView("Edit", proyecto);
        }

        // POST: ProyectoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Proyecto proyecto)
        {
            try
            {
                int result = await _proyectoBL.UpdateAsync(proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(proyecto);
            }
        }

        // GET: ProyectoController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var proyecto = await _proyectoBL.GetById(new Proyecto { Id = id });
            return PartialView("Delete", proyecto);

        }

        // POST: ProyectoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Proyecto proyecto)
        {
            try
            {
                await _proyectoBL.DeleteAsync(proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(proyecto);
            }
        }
    }
}
