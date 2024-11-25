﻿using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CommentController : Controller
    {
        private readonly CommentBL _commentBL;
        private readonly ProyectoBL _proyectoBL;
        private readonly UsuarioBL _usuarioBL;
        private readonly ProyectoUsuarioBL _proyectoUsuarioBL;


        public CommentController()
        {
            _commentBL = new CommentBL();
            _proyectoBL = new ProyectoBL();
            _usuarioBL = new UsuarioBL();
            _proyectoUsuarioBL = new ProyectoUsuarioBL();

        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> Index(int idProyecto)
        {
            // Obtener el ID del usuario logueado
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!User.IsInRole("Administrador"))
            {
                // Verificar si el usuario está unido al proyecto
                var usuariosUnidos = await _proyectoUsuarioBL.ObtenerUsuariosUnidosAsync(idProyecto);
                if (!usuariosUnidos.Any(u => u.Id == idUsuario))
                {
                    TempData["ErrorMessage"] = "No estás unido a este proyecto, no puedes ver los comentarios.";
                    return RedirectToAction("Index", "Proyecto"); // Redirigir a la vista de proyectos o a una página de error
                }
            }

            var comentarios = await _commentBL.ObtenerComentariosPorProyectoAsync(idProyecto);
            ViewBag.IdProyecto = idProyecto;

            return View(comentarios);
        }

        // Acción para crear un comentario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Create(int idProyecto, string contenido)
        {
            // Obtener el ID del usuario logueado
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!User.IsInRole("Administrador"))
            {
                // Obtener los usuarios unidos al proyecto
                var usuariosUnidos = await _proyectoUsuarioBL.ObtenerUsuariosUnidosAsync(idProyecto);
                // Verificar si el usuario está unido al proyecto
                if (!usuariosUnidos.Any(u => u.Id == idUsuario))
                {
                    TempData["ErrorMessage"] = "No estás unido a este proyecto, no puedes crear comentarios.";
                    return RedirectToAction("Index", "Proyecto"); // Redirigir a la vista de proyectos o a una página de error
                }
            }

            if (!string.IsNullOrEmpty(contenido))
            {
                // Crear el comentario a través del BL
                int result = await _commentBL.CrearComentarioAsync(idProyecto, idUsuario, contenido);

                // Verificar si el comentario se guardó correctamente
                if (result > 0)
                {
                    return RedirectToAction("Index", new { idProyecto });
                }
                else
                {
                    TempData["ErrorMessage"] = "Hubo un error al crear el comentario";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "El contenido del comentario no puede estar vacío";
            }

            return RedirectToAction("Index", new { idProyecto });
        }

        // Acción para eliminar un comentario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Delete(int idComentario, int idProyecto)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Eliminar el comentario a través del BL
            int result = await _commentBL.EliminarComentarioAsync(idComentario, idUsuario);

            if (result > 0)
            {
                TempData["InfoMessage"] = "Comentario eliminado";
            }
            else if (result == -1)
            {
                TempData["ErrorMessage"] = "No puedes eliminar un comentario que no te pertenece";
            }
            else if (result == -2)
            {
                TempData["ErrorMessage"] = "El tiempo para eliminar el comentario ha expirado (más de 15 minutos)";
            }
            else
            {
                TempData["ErrorMessage"] = "Hubo un error al eliminar el comentario";
            }

            return RedirectToAction("Index", new { idProyecto });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerComentarios(int idProyecto)
        {
            var comentarios = await _commentBL.ObtenerComentariosPorProyectoAsync(idProyecto);
            return PartialView("_Comentarios", comentarios);  // Devuelve solo la vista parcial
        }


    }
}

