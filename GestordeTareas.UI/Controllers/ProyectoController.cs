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
        private readonly ProyectoUsuarioBL _proyectoUsuarioBL;
        private readonly ProyectoBL _proyectoBL;
        private readonly UsuarioBL _usuarioBL;

        public ProyectoController()
        {
            _proyectoUsuarioBL = new ProyectoUsuarioBL();
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
        public async Task<ActionResult> Details(int id)
        {
            var proyecto = await _proyectoBL.GetById(new Proyecto { Id = id });

            if (proyecto == null)
            {
                return NotFound(); // Manejar el caso en que no se encuentra el proyecto
            }

            // Obtener la lista de usuarios unidos al proyecto
            var usuariosUnidos = await _proyectoUsuarioBL.ObtenerUsuariosUnidosAsync(id);

            // Pasar la lista de usuarios unidos a la vista
            ViewBag.UsuariosUnidos = usuariosUnidos;

            return View(proyecto);
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
                return Json(new { success = true, message = "Proyecto creado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al crear el proyecto: {ex.Message}" });
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
                return Json(new { success = true, message = "Proyecto editado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al editar el proyecto: {ex.Message}" });
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
                return Json(new { success = true, message = "Proyecto eliminado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al eliminar el proyecto: {ex.Message}" });
            }
        }
    }
}
