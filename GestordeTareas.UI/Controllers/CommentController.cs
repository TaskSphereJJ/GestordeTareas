using GestordeTaras.EN;
using GestordeTareas.BL;
using GestordeTareas.UI.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CommentController : Controller
    {
        private readonly CommentBL _commentBL;
        private readonly UsuarioBL _usuarioBL;
        private readonly ProyectoUsuarioBL _proyectoUsuarioBL;
        private readonly IHubContext<ChatHub> _hubContext;

        public CommentController(IHubContext<ChatHub> hubContext)
        {
            _commentBL = new CommentBL();
            _usuarioBL = new UsuarioBL();
            _proyectoUsuarioBL = new ProyectoUsuarioBL();
            _hubContext = hubContext;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index(int idProyecto)
        {
            try
            {
                var comentarios = await _commentBL.ObtenerComentariosPorProyectoAsync(idProyecto);
                ViewBag.IdProyecto = idProyecto;
                return View(comentarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la vista de comentarios: {ex.Message}");
                TempData["ErrorMessage"] = "Hubo un error al cargar los comentarios.";
                return RedirectToAction("Index", "Proyecto");
            }
        }
        // Acción para crear un comentario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            try
            {
                Console.WriteLine($"Datos recibidos: IdProyecto={comment.IdProyecto}, Content={comment.Content}");

                // Validar entrada nula o vacía
                if (comment == null || string.IsNullOrWhiteSpace(comment.Content))
                {
                    return BadRequest("El contenido del comentario no puede estar vacío.");
                }

                // Obtener el ID del usuario autenticado
                int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Verificar si el usuario está unido al proyecto
                var usuariosUnidos = await _proyectoUsuarioBL.ObtenerUsuariosUnidosAsync(comment.IdProyecto);
                if (!usuariosUnidos.Any(u => u.Id == idUsuario))
                {
                    return BadRequest("No estás unido a este proyecto, no puedes crear comentarios.");
                }

                // Asignar el usuario autenticado y la fecha al comentario
                comment.IdUsuario = idUsuario;
                comment.FechaComentario = DateTime.Now;

                // Guardar el comentario en la base de datos
                int result = await _commentBL.CrearComentarioAsync(comment.IdProyecto, comment.IdUsuario, comment.Content);

                if (result > 0)
                {
                    // Notificar al Hub para actualizar el chat
                    await _hubContext.Clients.All.SendAsync("RefreshChat");
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Error al guardar el comentario en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear comentario: {ex.Message}");
                return StatusCode(500, "Ocurrió un error inesperado al crear el comentario.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int idProyecto)
        {
            try
            {
                var comentarios = await _commentBL.ObtenerComentariosPorProyectoAsync(idProyecto);
                return PartialView("_ChatMessagesPartial", comentarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los comentarios: {ex.Message}");
                return StatusCode(500, "Error al obtener los comentarios.");
            }
        }
    }
}
