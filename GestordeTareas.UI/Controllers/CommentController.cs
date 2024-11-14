using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    public class CommentController : Controller
    {
        CommentBL _commentBL = new CommentBL();
        UsuarioBL _userbl = new UsuarioBL();

        public CommentController(CommentBL commentBL)
        {
            _commentBL = commentBL;
        }

        // Acción para cargar los comentarios del foro
        [HttpGet]
        public async Task<IActionResult> Foro(int idProyecto)
        {
            var comments = await _commentBL.GetCommentsByProject(idProyecto);
            ViewBag.IdProyecto = idProyecto;  // Pasar el Id del proyecto a la vista
            return View(comments);
        }

        // Acción para agregar un nuevo comentario
        [HttpPost]
        public async Task<IActionResult> CreateComments(Comment comment)
        {
            comment.FechaComentario = DateTime.Now;
            var idUser = User.Identity.IsAuthenticated;
            comment.IdUsuario = Convert.ToInt32(idUser);

            await _commentBL.CreateComment(comment);
            return RedirectToAction("Foro", new { idProyecto = comment.IdProyecto });
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateComments(Comment comment)
        //{
        //    // Establecer FechaComentario con la fecha actual
        //    DateTime fecha = DateTime.Now;
        //    string fechaISO = fecha.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //    comment.FechaComentario = fechaISO;

        //    // Verifica si el usuario está autenticado y asigna el IdUsuario
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var nombreUsuario = User.Identity.Name; // Obtiene el nombre de usuario (correo electrónico)

        //        // Crear un objeto Usuario (si es necesario)
        //        var usuario = new Usuario { NombreUsuario = nombreUsuario };

        //        // Ahora pasas el objeto Usuario en lugar de un string
        //        var usuarioObtenido = await _userbl.GetByNombreUsuarioAsync(usuario);

        //        if (usuarioObtenido != null)
        //        {
        //            // Asigna el IdUsuario del usuario obtenido
        //            comment.IdUsuario = usuarioObtenido.Id;
        //        }
        //        else
        //        {
        //            // Maneja el caso en que no se encuentra el usuario
        //            ModelState.AddModelError("", "Usuario no encontrado.");
        //            return View(comment);
        //        }
        //    }

        //    await _commentBL.CreateComment(comment);
        //    return RedirectToAction("Foro", new { idProyecto = comment.IdProyecto });
        //}



    }
}
