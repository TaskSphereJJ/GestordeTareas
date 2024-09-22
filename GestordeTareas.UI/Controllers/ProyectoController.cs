using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


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
        public async Task<ActionResult> Details(int id)
        {
            var proyecto = await _proyectoBL.GetById(new Proyecto { Id = id });

            if (proyecto == null)
            {
                return NotFound(); // Manejar el caso en que no se encuentra el proyecto
            }

            // Obtener la lista de usuarios unidos al proyecto
            var usuariosUnidos = await _proyectoBL.ObtenerUsuariosUnidosAsync(id);

            // Pasar la lista de usuarios unidos a la vista
            ViewBag.UsuariosUnidos = usuariosUnidos;

            return View(proyecto);
        }

        // Método para unir un usuario a un proyecto
        [HttpPost]
        public async Task<IActionResult> UnirUsuarioAProyecto(int idProyecto)
        {
            var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
            var actualUser = users.FirstOrDefault();

            if (actualUser == null)
            {
                return NotFound(); // Usuario no encontrado
            }

            int idUsuario = actualUser.Id;

            // Verificar si el usuario ya está unido al proyecto
            var usuariosUnidos = await _proyectoBL.ObtenerUsuariosUnidosAsync(idProyecto);
            if (usuariosUnidos.Any(u => u.Id == idUsuario))
            {
                TempData["ErrorMessage"] = "Ya perteneces a este proyecto.";
                return RedirectToAction("Details", new { id = idProyecto }); // Volver a la vista de detalles del proyecto
            }

            var result = await ProyectoBL.UnirUsuarioAProyectoAsync(idProyecto, idUsuario);
            if (result > 0)
            {
                TempData["SuccessMessage"] = "Te has unido al proyecto exitosamente.";
                return RedirectToAction("Details", new { id = idProyecto }); // Redirigir a detalles del proyecto
            }

            TempData["ErrorMessage"] = "No se pudo unir al proyecto.";
            return RedirectToAction("Details", new { id = idProyecto });
        }

        // Método para que un usuario salga de un proyecto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirDeProyecto(int idProyecto)
        {
            var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
            var actualUser = users.FirstOrDefault();

            if (actualUser == null)
            {
                return NotFound(); // Manejar el caso en que no se encuentra el usuario autenticado
            }

            int idUsuario = actualUser.Id;

            // Llamar al método para eliminar el usuario del proyecto
            var result = await ProyectoBL.EliminarUsuarioDeProyectoAsync(idProyecto, idUsuario);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Has salido del proyecto exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo salir del proyecto.";
            }

            return RedirectToAction("Index", "Proyecto"); 
        }


        // Método para obtener proyectos por usuario
        public async Task<IActionResult> MisProyectos()
        {
            // Obtener el ID del usuario actual
            var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
            var actualUser = users.FirstOrDefault();

            if (actualUser == null)
            {
                return NotFound(); // Manejar el caso en que no se encuentra el usuario autenticado
            }

            int idUsuario = actualUser.Id; // Obtener el ID del usuario actual

            List<Proyecto> proyectos = await ProyectoBL.ObtenerProyectosPorUsuarioAsync(idUsuario);
            return View(proyectos);
        }


        // Método para obtener proyectos disponibles
        public async Task<IActionResult> ProyectosDisponibles()
        {
            // Obtener el ID del usuario actual
            var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
            var actualUser = users.FirstOrDefault();

            if (actualUser == null)
            {
                return NotFound(); // Manejar el caso en que no se encuentra el usuario autenticado
            }

            int idUsuario = actualUser.Id; // Obtener el ID del usuario actual

            List<Proyecto> proyectosDisponibles = await ProyectoBL.ObtenerProyectosDisponiblesAsync(idUsuario);
            return View(proyectosDisponibles);
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
