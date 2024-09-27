//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using GestordeTareas.DAL;
//using GestordeTaras.EN;
//using GestordeTareas.BL;
//using Microsoft.AspNetCore.Authorization;

//namespace GestordeTareas.UI.Controllers
//{
//    [Authorize]
//    public class ElegirTareaController : Controller
//    {
//        private readonly ContextoBD _context;
//        private readonly UsuarioBL _usuarioBL;
//        private readonly ElegirTareaBL _elegirTareaBL;

//        public ElegirTareaController(ContextoBD context, UsuarioBL usuarioBL, ElegirTareaBL elegirTareaBL)
//        {
//            _context = context;
//            _usuarioBL = usuarioBL;
//            _elegirTareaBL = elegirTareaBL;
//        }

//        // GET: ElegirTarea
//        public async Task<IActionResult> Index()
//        {
//            var idUsuario = await ObtenerIdUsuarioActualAsync();
//            if (idUsuario == null)
//            {
//                return RedirectToAction("Login", "Account");
//            }

//            // Obtener los proyectos a los que el usuario está asignado
//            var proyectosUsuario = await _context.ProyectoUsuario
//                .Where(pu => pu.IdUsuario == idUsuario)
//                .Select(pu => pu.IdProyecto)
//                .ToListAsync();

//            // Obtener tareas disponibles de los proyectos del usuario
//            var tareasDisponibles = await _context.Tarea
//                .Where(t => proyectosUsuario.Contains(t.IdProyecto) &&
//                            !_context.ElegirTarea.Any(et => et.IdTarea == t.Id))
//                .ToListAsync();

//            ViewData["IdTarea"] = new SelectList(tareasDisponibles, "Id", "Nombre");
//            ViewData["IdProyecto"] = new SelectList(_context.Proyecto.Where(p => proyectosUsuario.Contains(p.Id)), "Id", "Nombre");

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Elegir(int idTarea, int idProyecto)
//        {
//            var idUsuario = await ObtenerIdUsuarioActualAsync();
//            if (idUsuario == null)
//            {
//                return Json(new { success = false, message = "Debes iniciar sesión para elegir una tarea." });
//            }

//            try
//            {
//                bool result = await _elegirTareaBL.ElegirTareaAsync(idTarea, idUsuario.Value, idProyecto);

//                if (result)
//                {
//                    return Json(new { success = true, message = "Has elegido la tarea con éxito." });
//                }
//                else
//                {
//                    return Json(new { success = false, message = "No se pudo elegir la tarea. Por favor, inténtalo de nuevo." });
//                }
//            }
//            catch (ArgumentException ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//            catch (Exception)
//            {
//                return Json(new { success = false, message = "Ocurrió un error al elegir la tarea. Por favor, inténtalo de nuevo." });
//            }
//        }

//        // Método para obtener el ID del usuario actual
//        private async Task<int?> ObtenerIdUsuarioActualAsync()
//        {
//            var usuario = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name });
//            return usuario.FirstOrDefault()?.Id;
//        }
//    }
//}
