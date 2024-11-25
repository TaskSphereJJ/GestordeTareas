using GestordeTareas.BL;
using GestordeTareas.UI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UsuarioBL _usuarioBL;
        private readonly ProyectoBL _proyectoBL;
        private readonly TareaBL _tareaBL;
        private readonly ProyectoUsuarioBL _proyectoUsuarioBL;



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _usuarioBL = new UsuarioBL();
            _proyectoBL = new ProyectoBL();
            _tareaBL = new TareaBL();
            _proyectoUsuarioBL = new ProyectoUsuarioBL();
        }

        public async Task<IActionResult> Index()
        {
            // Obtener los datos de usuarios, proyectos, tareas y los usuarios unidos a los proyectos
            var usuarios = await _usuarioBL.GetAllAsync();
            var proyectos = await _proyectoBL.GetAllAsync();
            var tareas = await _tareaBL.GetAllAsync();
            var usuariosPorProyecto = await _proyectoUsuarioBL.ObtenerTodosAsync();

            // Obtener el total de usuarios, proyectos y tareas
            ViewBag.TotalUsuarios = usuarios.Count;
            ViewBag.TotalProyectos = proyectos.Count;
            ViewBag.TotalTareas = tareas.Count;

            // Preparar un diccionario para las tareas por proyecto
            var tareasPorProyecto = proyectos.ToDictionary(
                p => p.Titulo,  // Título del proyecto
                p => new
                {
                    TotalTareas = tareas.Count(t => t.IdProyecto == p.Id), // Total de tareas por proyecto
                    Pendientes = tareas.Count(t => t.IdProyecto == p.Id && t.EstadoTarea != null && t.EstadoTarea.Nombre == "Pendiente"),
                    EnProceso = tareas.Count(t => t.IdProyecto == p.Id && t.EstadoTarea != null && t.EstadoTarea.Nombre == "En Proceso"),
                    Finalizadas = tareas.Count(t => t.IdProyecto == p.Id && t.EstadoTarea != null && t.EstadoTarea.Nombre == "Finalizada")
                }
            );

            // Calcular la barra de progreso por proyecto basado en las tareas finalizadas
            var progresoPorProyecto = proyectos.ToDictionary(
                p => p.Titulo,
                p =>
                {
                    var totalTareas = tareas.Count(t => t.IdProyecto == p.Id);
                    var tareasFinalizadas = tareas.Count(t => t.IdProyecto == p.Id && t.EstadoTarea != null && t.EstadoTarea.Nombre == "Finalizada");

                    return totalTareas == 0 ? 0 : (int)((double)tareasFinalizadas / totalTareas * 100); // Porcentaje de progreso
                }
            );

            // Recuperar los usuarios unidos a los proyectos (relación ProyectoUsuario)
            var usuariosPorProyectoDiccionario = new Dictionary<string, int>();
            foreach (var proyecto in proyectos)
            {
                var usuariosDelProyectoCount = usuariosPorProyecto
                    .Count(pu => pu.IdProyecto == proyecto.Id); // Contamos cuántos usuarios están unidos al proyecto

                usuariosPorProyectoDiccionario.Add(proyecto.Titulo, usuariosDelProyectoCount);
            }

            // Pasar los datos a la vista a través de ViewBag
            ViewBag.TareasPorProyecto = tareasPorProyecto;
            ViewBag.ProgresoPorProyecto = progresoPorProyecto;
            ViewBag.UsuariosPorProyecto = usuariosPorProyectoDiccionario;

            return View();
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}