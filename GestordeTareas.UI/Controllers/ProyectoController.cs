using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;



namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ProyectoController : Controller
    {
        private readonly ProyectoUsuarioBL _proyectoUsuarioBL;
        private readonly ProyectoBL _proyectoBL;
        private readonly UsuarioBL _usuarioBL;
        private readonly InvitacionProyectoBL _invitacionProyectoBL;
        private readonly EmailService _emailService;

        public ProyectoController(EmailService emailService)
        {
            _proyectoUsuarioBL = new ProyectoUsuarioBL();
            _proyectoBL = new ProyectoBL();
            _usuarioBL = new UsuarioBL();
            _invitacionProyectoBL = new InvitacionProyectoBL();
            _emailService = emailService;
        }

        // GET: ProyectoController
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> Index(string filter, string query)
        {
            List<Proyecto> Lista;

            if (string.IsNullOrEmpty(query))
            {
                // Si no se proporciona un término de búsqueda, obtiene todos los proyectos
                Lista = await _proyectoBL.GetAllAsync();
            }
            else
            {
                // Busca usando el método que permite buscar por título o por nombre del administrador
                Lista = await _proyectoBL.BuscarPorTituloOAdministradorAsync(query);
            }

            return View(Lista);
        }



        // GET: ProyectoController/Details/5
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> Details(int id)
        {
            var proyecto = await _proyectoBL.GetById(new Proyecto { Id = id });

            if (proyecto == null)
            {
                return NotFound(); 
            }

            // SE OBTIENE LA LISTA DE USUARIOS UNIDOS AL PROYECTO
            var usuariosUnidos = await _proyectoUsuarioBL.ObtenerUsuariosUnidosAsync(id);
            // SE PASA LA LISTA DE USUARIOS UNIDOS A LA VISTA
            ViewBag.UsuariosUnidos = usuariosUnidos;

            // SE OBTIENE EL ENCARGADO DEL PROYECTO
            var encargado = await _proyectoUsuarioBL.ObtenerEncargadoPorProyectoAsync(id);
            ViewBag.Encargado = encargado;

            // Obtener el ID del usuario actual
            int idUsuarioActual = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // SE VERIFICA SI EL USUARIO ES ENCARGADO
            bool esEncargado = await _proyectoUsuarioBL.IsUsuarioEncargadoAsync(id, idUsuarioActual);
            ViewBag.EsEncargado = esEncargado;

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
                // OBTENER EL IDUSUARIO DEL USUARIO AUTENTICADO
                var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
                var actualUser = users.FirstOrDefault();

                // ASIGNAR EL IDUSUARIO AL PROYECTO
                proyecto.IdUsuario = actualUser.Id;

                // GENERA UN CÓDIGO DE ACCESO ÚNICO
                string codigoAcceso;
                do
                {
                    codigoAcceso = _proyectoBL.GenerarCodigoAcceso();
                }
                while (await _proyectoBL.ExisteCodigoAccesoAsync(codigoAcceso)); // SE VERIFICA QUE EL CÓDIGO NO EXISTA

                // Asignar el código generado al proyecto
                proyecto.CodigoAcceso = codigoAcceso;

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

        // MÉTODO PARA ASIGNAR UN USUARIO COMO ENCARGADO DE UN PROYECTO 
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> AsignarEncargado(int idProyecto, int idUsuario)
        {
            try
            {
                bool resultado = await _proyectoUsuarioBL.AsignarEncargadoAsync(idProyecto, idUsuario);

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

            return RedirectToAction("Details", new { id = idProyecto });
        }


        //METODO QUE PERMITE AL ADMINISTRADOR ENVIAR INVITACIONES PARA PROYECTOS
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EnviarInvitacion(InvitacionProyecto invitacion)
        {
            invitacion.FechaCreacion = DateTime.Now;
            invitacion.FechaExpiracion = DateTime.Now.AddDays(7); 
            invitacion.Estado = "Pendiente"; // ESTADO INICIAL DE LA INVITACION
            invitacion.Token = Guid.NewGuid().ToString(); // GENERA UN TOKEN ÚNICO

            if (true)
            {
                try
                {
                    // SE VERIFICA QUE EL PROYECTO EXISTA
                    var proyecto = await _proyectoBL.GetById(new Proyecto { Id = invitacion.IdProyecto });
                    if (proyecto == null)
                    {
                        TempData["ErrorMessage"] = "El proyecto no existe.";
                        return RedirectToAction("Details", new { id = invitacion.IdProyecto });
                    }

                    // INTENTAR ENVIAR LA INVITACIÓN
                    var result = await _invitacionProyectoBL.EnviarInvitacionAsync(invitacion);
                    if (result > 0)
                    {
                        // URL LOCAL DE LA APLICACION
                        string baseUrl = "https://localhost:7297";

                        // ENLACES DE INVITACIÓN CON EL TOKEN Y LA DECISIÓN (ACEPTAR O RECHAZAR)
                        string enlaceAceptar = $"{baseUrl}/Proyecto/AceptarInvitacion?token={invitacion.Token}&decision=aceptar";
                        string enlaceRechazar = $"{baseUrl}/Proyecto/AceptarInvitacion?token={invitacion.Token}&decision=rechazar";

                        // SE GENERA EL CUERPO DEL CORREO CON EL NUEVO METODO EN EMAILSERVICE
                        string cuerpo = _emailService.GenerateInvitationEmailBody(invitacion.CorreoElectronico, proyecto.Titulo, enlaceAceptar, enlaceRechazar, invitacion.FechaExpiracion);

                        // SE ENVIA EL CORREO CON EL MENSAJE 
                        await _emailService.SendEmailAsync(invitacion.CorreoElectronico, "Invitación a un proyecto", cuerpo);


                        TempData["SuccessMessage"] = "Invitación enviada correctamente.";
                        return RedirectToAction("Invitaciones", new { id = invitacion.IdProyecto });
                    }

                    TempData["ErrorMessage"] = "Hubo un problema al enviar la invitación.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error al enviar la invitación: " + ex.Message;
                }
            }

            TempData["ErrorMessage"] = "Los datos de la invitación son inválidos.";
            return RedirectToAction("Invitaciones", new { id = invitacion.IdProyecto });
        }



        // METODO PARA ACEPTAR O RECHAZAR LA INVITACION AL PROYECTO
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AceptarInvitacion(string token, string decision)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "El token no puede estar vacío.";
                return RedirectToAction("Index");
            }

            // SE VERIFICA SI EL USUARIO ESTÁ AUTENTICADO
            if (!User.Identity.IsAuthenticated)
            {
                // SE GUARDA EL TOKEN Y LA DECISION EN TEMPDATA PARA USARLO EN EL LOGIN
                TempData["Token"] = token;
                TempData["Decision"] = decision;
                return RedirectToAction("Login", "Usuario");
            }

            // SE OBTIENE EL ID DEL USUARIO LOGUEADO Y EL CORREO
            int idUsuario = GetLoggedUserId();
            string correoUsuario = User.Identity.Name;

            // SE VERIFICA SI LA INVITACION EXISTE
            var invitacion = await _invitacionProyectoBL.ObtenerPorTokenAsync(token);
            if (invitacion == null)
            {
                TempData["ErrorMessage"] = "La invitación no existe o es inválida.";
                return RedirectToAction("Index");
            }

            // SE VERIFICA SI EL CORREO DEL USUARIO COINCIDE CON EL CORREO DE LA INVITACION
            if (invitacion.CorreoElectronico != correoUsuario)
            {
                TempData["ErrorMessage"] = "El correo electrónico no coincide con la invitación.";
                return RedirectToAction("Index");
            }

            if (decision == "aceptar")
            {
                // ACEPTAR LA INVITACION Y UNIR EL USUARIO AL PROYECTO INVITADO
                var result = await _invitacionProyectoBL.AceptarInvitacionAsync(token, idUsuario, correoUsuario);
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Has aceptado la invitación y te has unido al proyecto.";
                }
                else if (result == -1)
                {
                    TempData["ErrorMessage"] = "Ya perteneces a este proyecto.";
                }
                else if (result == -3)
                {
                    TempData["ErrorMessage"] = "La invitación ya ha sido procesada.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo aceptar la invitación.";
                }
            }
            else if (decision == "rechazar")
            {
                // SE VERIFICA SI LA INVITACION YA HA SIDO PROCESADA (ACEPTADA O RECHAZADA)
                if (invitacion.Estado != "Pendiente")
                {
                    TempData["ErrorMessage"] = "No puedes rechazar esta invitación porque ya ha sido procesada.";
                }
                else
                {
                    // SE RECHAZA LA INVITACION
                    var result = await _invitacionProyectoBL.RechazarInvitacionAsync(token, idUsuario, correoUsuario);
                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = "Invitación rechazada correctamente.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No se pudo rechazar la invitación.";
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Decisión no válida.";
            }

            // LIMPIA LOS TEMPDATA LUEGO DE PROCESAR LA INVITACION
            TempData.Remove("Token");
            TempData.Remove("Decision");

            return RedirectToAction("Index");
        }


        //MÉTODO PARA LIMPIAR O ELIMINAR LAS INVITACIONES 
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarInvitacion(int id, int idProyecto)
        {
            try
            {
                bool eliminado = await _invitacionProyectoBL.LimpiarInvitacionPorIdAsync(id);
                if (eliminado)
                {
                    TempData["SuccessMessage"] = " La invitación ha sido eliminada.";
                }
                else
                {
                    TempData["InfoMessage"] = "No se encontró la invitación o no se puede eliminar.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar la invitación: " + ex.Message;
            }

            return RedirectToAction("Invitaciones", new { id = idProyecto });
        }


        // Método GET para mostrar las invitaciones de un proyecto específico
        [HttpGet]
        public async Task<IActionResult> Invitaciones(int id)
        {
            try
            {
                // Obtener las invitaciones para el proyecto con el ID especificado
                var invitaciones = await _invitacionProyectoBL.ObtenerInvitacionesPorProyectoAsync(id);

                // Verificar si se encontraron invitaciones
                if (invitaciones == null || !invitaciones.Any())
                {
                    TempData["InfoMessage"] = "No hay invitaciones para mostrar.";
                }

                ViewBag.IdProyecto = id;
                // Retornar la vista con las invitaciones
                return View(invitaciones);
            }
            catch (Exception ex)
            {
                // Registrar el error en el log (puedes usar ILogger)
                Console.WriteLine($"Error: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error al obtener las invitaciones.";
                return View("Error"); // Cambia a tu vista de error
            }
        }


        // Método para filtrar invitaciones por estado
        [HttpGet]
        public async Task<IActionResult> FiltrarInvitaciones(int id, string estado)
        {
            try
            {
                IEnumerable<InvitacionProyecto> invitaciones;

                if (estado == "Todos")
                {
                    // Obtener todas las invitaciones sin filtrar
                    invitaciones = await _invitacionProyectoBL.ObtenerInvitacionesPorProyectoAsync(id);
                }
                else
                {
                    // Obtener las invitaciones filtradas por estado
                    invitaciones = await _invitacionProyectoBL.ObtenerInvitacionesPorEstadoAsync(id, new List<string> { estado });
                }
                // Verificar si se encontraron invitaciones
                if (invitaciones == null || !invitaciones.Any())
                {
                    TempData["InfoMessage"] = "No hay invitaciones que coincidan con el filtro.";
                }

                ViewBag.IdProyecto = id;
                ViewBag.Estado = estado;
                // Retornar la vista con las invitaciones filtradas
                return View("Invitaciones",invitaciones);
            }

            catch (Exception ex)
            {
                // Registrar el error en el log (puedes usar ILogger)
                Console.WriteLine($"Error: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error al filtrar las invitaciones.";
                return View("Error"); // Cambia a tu vista de error
            }
        }


        // Método para obtener el ID del usuario logueado 
        private int GetLoggedUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }


    }
}
