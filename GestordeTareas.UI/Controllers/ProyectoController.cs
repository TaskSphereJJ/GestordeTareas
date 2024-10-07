using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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

            // Obtener el encargado del proyecto
            var encargado = await _proyectoUsuarioBL.ObtenerEncargadoPorProyectoAsync(id);
            // Pasar el encargado a la vista
            ViewBag.Encargado = encargado;

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
        [Authorize(Roles = "Administrador")]
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
                TempData["SuccessMessage"] = "Proyecto creado correctamente.";
                return Json(new { success = true, message = "Proyecto creado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["ErrorMessage"] = "Hubo un problema al crear el proyecto.";
                return Json(new { success = false, message = "Hubo un problema al crear el proyecto: " + ex.Message });
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
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int id, Proyecto proyecto)
        {
            try
            {
                int result = await _proyectoBL.UpdateAsync(proyecto);
                TempData["SuccessMessage"] = "Proyecto actualizado correctamente.";
                return Json(new { success = true, message = "Proyecto actualizado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["ErrorMessage"] = "Hubo un problema al actualizar el proyecto.";
                return Json(new { success = false, message = "Hubo un problema al actualizar el proyecto: " + ex.Message });
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
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id, Proyecto proyecto)
        {
            try
            {
                await _proyectoBL.DeleteAsync(proyecto);
                TempData["SuccessMessage"] = "Proyecto eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["ErrorMessage"] = "Hubo un problema al eliminar el proyecto.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Método para asignar un usuario como encargado de un proyecto
        [HttpPost]
        [Authorize(Roles = "Administrador")] // Solo un administrador puede asignar encargados
        public async Task<ActionResult> AsignarEncargado(int idProyecto, int idUsuario)
        {
            try
            {
                bool resultado = await ProyectoUsuarioBL.AsignarEncargadoAsync(idProyecto, idUsuario);

                if (resultado)
                {
                    TempData["SuccessMessage"] = "Usuario asignado como encargado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ya existe un encargado para este proyecto.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hubo un problema al asignar el encargado: " + ex.Message;
            }

            // Redirigir a la vista de detalles del proyecto
            return RedirectToAction("Details", new { id = idProyecto });
        }

    }
}
