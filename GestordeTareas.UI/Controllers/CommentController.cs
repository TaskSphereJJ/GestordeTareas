﻿using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestordeTareas.UI.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentBL _commentBL;
        private readonly ProyectoBL _proyectoBL;
        private readonly UsuarioBL _usuarioBL;

        public CommentController()
        {
            _commentBL = new CommentBL();
            _proyectoBL = new ProyectoBL();
            _usuarioBL = new UsuarioBL();
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> Index(int idProyecto)
        {
            var comentarios = await _commentBL.ObtenerComentariosPorProyectoAsync(idProyecto);
            ViewBag.IdProyecto = idProyecto;
            return View(comentarios);
        }

        // Acción para crear un comentario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int idProyecto, string contenido)
        {
            // Obtener el ID del usuario logueado
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!string.IsNullOrEmpty(contenido))
            {
                // Crear el comentario a través del BL
                int result = await _commentBL.CrearComentarioAsync(idProyecto, idUsuario, contenido);

                // Verificar si el comentario se guardó correctamente
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Comentario creado con éxito.";
                    return RedirectToAction("Index", new { idProyecto });
                }
                else
                {
                    TempData["ErrorMessage"] = "Hubo un error al crear el comentario.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "El contenido del comentario no puede estar vacío.";
            }

            return RedirectToAction("Index", new { idProyecto });
        }

        // Acción para eliminar un comentario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int idComentario, int idProyecto)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Eliminar el comentario a través del BL
            int result = await _commentBL.EliminarComentarioAsync(idComentario, idUsuario);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Comentario eliminado con éxito.";
            }
            else if (result == -1)
            {
                TempData["ErrorMessage"] = "No puedes eliminar un comentario que no te pertenece.";
            }
            else if (result == -2)
            {
                TempData["ErrorMessage"] = "El tiempo para eliminar el comentario ha expirado (más de 15 minutos).";
            }
            else
            {
                TempData["ErrorMessage"] = "Hubo un error al eliminar el comentario.";
            }

            return RedirectToAction("Index", new { idProyecto });
        }

    }
}

