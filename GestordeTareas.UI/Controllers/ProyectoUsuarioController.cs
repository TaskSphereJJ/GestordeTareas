using GestordeTaras.EN;
using GestordeTareas.BL;
using GestordeTareas.DAL;
using Humanizer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestordeTareas.UI.Controllers
{
    public class ProyectoUsuarioController : Controller
    {
        private readonly ProyectoUsuarioBL _proyectoUsuarioBL;
        private readonly UsuarioBL _usuarioBL;

        public ProyectoUsuarioController()
        {
            _proyectoUsuarioBL = new ProyectoUsuarioBL();
            _usuarioBL = new UsuarioBL();
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
            var usuariosUnidos = await _proyectoUsuarioBL.ObtenerUsuariosUnidosAsync(idProyecto);
            if (usuariosUnidos.Any(u => u.Id == idUsuario))
            {
                TempData["ErrorMessage"] = "Ya perteneces a este proyecto.";
                return RedirectToAction("Details", "Proyecto", new { id = idProyecto }); // Volver a la vista de detalles del proyecto
            }

            var result = await ProyectoUsuarioBL.UnirUsuarioAProyectoAsync(idProyecto, idUsuario);
            if (result > 0)
            {
                TempData["SuccessMessage"] = "Te has unido al proyecto exitosamente.";
                return RedirectToAction("Details", "Proyecto", new { id = idProyecto }); // Redirigir a detalles del proyecto
            }

            TempData["ErrorMessage"] = "No se pudo unir al proyecto.";
            return RedirectToAction("Details", "Proyecto", new { id = idProyecto });
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
            var result = await ProyectoUsuarioBL.EliminarUsuarioDeProyectoAsync(idProyecto, idUsuario);

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

            List<Proyecto> proyectos = await ProyectoUsuarioBL.ObtenerProyectosPorUsuarioAsync(idUsuario);
            return View("MisProyectos",proyectos);
        }

    }
}
