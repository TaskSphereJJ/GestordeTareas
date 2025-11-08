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
using System.Security.Claims;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class TareaController : Controller
    {
        private readonly TareaBL _tareaBL;
        private readonly CategoriaBL _categoriaBL;
        private readonly PrioridadBL _prioridadBL;
        private readonly EstadoTareaBL _estadoTareaBL;
        private readonly ProyectoBL _proyectoBL;
        private readonly UsuarioBL _usuarioBL;
        private readonly ProyectoUsuarioBL _proyectoUsuarioBL;
        private readonly ElegirTareaBL _elegirTareaBL;

        public TareaController()
        {
            _tareaBL = new TareaBL();
            _categoriaBL = new CategoriaBL();
            _prioridadBL = new PrioridadBL();
            _estadoTareaBL = new EstadoTareaBL();
            _proyectoBL = new ProyectoBL();
            _usuarioBL = new UsuarioBL();
            _proyectoUsuarioBL = new ProyectoUsuarioBL();
            _elegirTareaBL = new ElegirTareaBL();
        }

        // GET: TareaController
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> Index(int proyectoId)
        {
            if (!await VerificarAcceso(proyectoId))
            {
                TempData["ErrorMessage"] = "No tienes acceso a este proyecto";
                return RedirectToAction("Index", "Proyecto"); // Redirigir a la vista de proyectos
            }

            // Aquí cargas las tareas asociadas al proyecto con el ID proporcionado
            var tareas = await _tareaBL.GetTareasByProyectoIdAsync(proyectoId);
            ViewBag.ProyectoId = proyectoId;

            // Obtener el ID del usuario actual
            int idUsuarioActual = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            // Verificar si el usuario es encargado
            bool esEncargado = await _proyectoUsuarioBL.IsUsuarioEncargadoAsync(proyectoId, idUsuarioActual);
            ViewBag.EsEncargado = esEncargado;

            return View(tareas);
        }

        // GET: TareaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            return PartialView("Details", tarea);
        }
        // Nuevo
        private async Task<int> GetProyectoIdAsync(Proyecto proyecto)
        {
            var result = await _proyectoBL.GetByIdAsync(proyecto);
            int proyectoId = Convert.ToInt32(result);
            return proyectoId;
        }

        // GET: TareaController/Create
        public async Task<ActionResult> Create(int idProyecto)
        {
            // Cargar las listas desplegables
            await LoadDropDownListsAsync();

            // Crear una nueva instancia de Tarea con el IdProyecto proporcionado
            var tarea = new Tarea { IdProyecto = idProyecto };

            // Pasar el IdProyecto a la vista a través del ViewBag
            ViewBag.ProyectoId = tarea.IdProyecto;
            ViewBag.EstadoPendienteId = await EstadoTareaDAL.GetEstadoPendienteIdAsync();

            // Devolver una vista parcial con el modelo de tarea
            return PartialView("Create", tarea);
        }

        // POST: TareaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Colaborador")]
        public async Task<ActionResult> Create(Tarea tarea, int idProyecto)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Método que obtiene el ID del usuario actual

            if (!User.IsInRole("Administrador"))
            {
                // Verificar si el usuario es el encargado del proyecto
                bool esEncargado = await _proyectoUsuarioBL.IsUsuarioEncargadoAsync(tarea.IdProyecto, idUsuario);

                if (!esEncargado)
                {
                    TempData["ErrorMessage"] = "No tienes permisos para realizar cambios";
                    return Json(new { success = false, message = "No tienes permisos para realizar cambios" });
                }
            }
            try
            {
                // Asignar el ID del proyecto a la tarea
                tarea.IdProyecto = idProyecto;
                tarea.FechaCreacion = DateTime.Now;
                int estadoPendienteId = await EstadoTareaDAL.GetEstadoPendienteIdAsync();
                tarea.IdEstadoTarea = estadoPendienteId;

                int result = await _tareaBL.CreateAsync(tarea);
                TempData["SuccessMessage"] = "Tarea creada correctamente.";
                return Json(new { success = true, message = "Tarea creada correctamente", id = idProyecto });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear la tarea: " + ex.Message });
            }
        }

        //MÉTODO PARA CARGAR LISTAS DESPLEGABLES SELECCIONABLES 
        private async Task LoadDropDownListsAsync()
        {
            // Obtener todos los datos de cada una disponibles
            var categorias = await _categoriaBL.GetAllAsync();
            var prioridades = await _prioridadBL.GetAllAsync();
            var estadosTarea = await _estadoTareaBL.GetAllAsync();
            var proyectos = await _proyectoBL.GetAllAsync();

            // Se crean SelectList para cada entidad con las propiedades Id como valor y Nombre como texto visible
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");
            ViewBag.Prioridades = new SelectList(prioridades, "Id", "Nombre");
            ViewBag.EstadosTarea = new SelectList(estadosTarea, "Id", "Nombre");
            ViewBag.Proyectos = new SelectList(proyectos, "Id", "Titulo");
        }


        // GET: TareaController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            await LoadDropDownListsAsync(); //Se llama al método y se espera que cargue
            return PartialView("Edit", tarea);
        }

        // POST: TareaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Colaborador")]
        public async Task<ActionResult> Edit(int id, Tarea tarea)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 

            if (!User.IsInRole("Administrador"))
            {      
                // Verificar si el usuario es el encargado del proyecto
                bool esEncargado = await _proyectoUsuarioBL.IsUsuarioEncargadoAsync(tarea.IdProyecto, idUsuario);

                if (!esEncargado)
                {
                    TempData["ErrorMessage"] = "No tienes permisos para realizar cambios";
                    return Json(new { success = false, message = "No tienes permisos para realizar cambios" });
                }
            }

            try
            {
                int result = await _tareaBL.UpdateAsync(tarea);
                TempData["SuccessMessage"] = "Tarea modificada correctamente.";
                return Json(new { success = true, message = "Tarea modificada correctamente", id = tarea.IdProyecto });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al modificadar la tarea: {ex.Message}" });
            }
        }


        // GET: TareaController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            return PartialView("Delete", tarea);
        }

        // POST: TareaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Colaborador")]
        public async Task<ActionResult> Delete(int id, Tarea tarea)
        {
            int idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Obtener la tarea por ID
            var tareaObtenida = await _tareaBL.GetById(new Tarea { Id = id });
            if (tareaObtenida == null)
            {
                return NotFound("Tarea no encontrada");
            }

            if (!User.IsInRole("Administrador"))
            {
                // Verificar si el usuario es el encargado del proyecto
                bool esEncargado = await _proyectoUsuarioBL.IsUsuarioEncargadoAsync(tareaObtenida.IdProyecto, idUsuario);

                if (!esEncargado)
                {
                    TempData["ErrorMessage"] = "No tienes permisos para realizar cambios";
                    return RedirectToAction("Index");
                }
            }
            try
            {
                await _tareaBL.DeleteAsync(tareaObtenida);
                TempData["SuccessMessage"] = "Tarea eliminada correctamente";
                return RedirectToAction("Index", new { proyectoId = tareaObtenida.IdProyecto });

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["ErrorMessage"] = "Hubo un problema al eliminar la tarea";
                return Json(new { success = false, message = $"Error al eliminar la tarea: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("Tarea/update-state")]
        public async Task<IActionResult> ActualizarEstadoTarea([FromBody] TareaUpdateModel model)
        {
            try
            {
                using (var bdContexto = new ContextoBD())
                {
                    var tareaBD = await bdContexto.Tarea.FirstOrDefaultAsync(t => t.Id == model.IdTarea);
                    if (tareaBD != null)
                    {
                        var estadoValido = await bdContexto.EstadoTarea.FindAsync(model.IdEstadoTarea);
                        if (estadoValido == null)
                        {
                            return BadRequest("Estado no válido");
                        }
                            

                        tareaBD.IdEstadoTarea = model.IdEstadoTarea;
                        bdContexto.Update(tareaBD);
                        await bdContexto.SaveChangesAsync();
                        return Ok(new { nombreEstado = estadoValido.Nombre });
                    }
                    else
                    {
                        return NotFound("Tarea no encontrada");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        public class TareaUpdateModel
        {
            public int IdTarea { get; set; }
            public int IdEstadoTarea { get; set; }
        }

        // MÉTODO PARA VERIFICAR EL ACCESO A LAS TAREAS
        private async Task<bool> VerificarAcceso(int idProyecto)
        {
            var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
            var actualUser = users.FirstOrDefault();

            if (actualUser == null)
            {
                return false; // Usuario no encontrado
            }

            int idUsuario = actualUser.Id;

            // Verificar si el usuario es administrador
            bool esAdmin = User.IsInRole("Administrador");
            if (esAdmin)
            {
                return true; // Acceso permitido para administradores
            }

            // Verificar si el usuario está unido al proyecto
            var usuariosUnidos = await _proyectoUsuarioBL.ObtenerUsuariosUnidosAsync(idProyecto);
            return usuariosUnidos.Any(u => u.Id == idUsuario); // Devuelve true si está unido, false si no
        }

        // POST: TareaController/ElegirTarea
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ElegirTarea(int idTarea)
        {
            try
            {
                // Obtén el ID del usuario actual
                var usuario = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
                var idUsuario = usuario.FirstOrDefault()?.Id;

                // Verificar si el usuario existe
                if (idUsuario == null)
                {
                    TempData["ErrorMessage"] = "Usuario no encontrado";
                    return View("Index");
                }

                // Obtener la tarea por ID
                Tarea tarea = await _tareaBL.GetById(new Tarea { Id = idTarea });

                // Verificar si la tarea existe
                if (tarea == null)
                {
                    TempData["ErrorMessage"] = "Tarea no encontrada";
                    return View("Index");
                }

                // Verificar si la tarea está en estado "Pendiente"
                if (tarea.EstadoTarea.Nombre != "Pendiente")
                {
                    TempData["ErrorMessage"] = "La tarea no está en Disponible";
                    return View("Index");
                }

                // Asignar la tarea al usuario
                var resultado = await _elegirTareaBL.ElegirTareaAsync(idTarea, idUsuario.Value, tarea.IdProyecto);

                if (resultado)
                {
                    // Actualizar estado de la tarea a "En Proceso"
                    const int ID_ESTADO_EN_PROCESO = 2;
                    await _tareaBL.ActualizarEstadoTareaAsync(idTarea, ID_ESTADO_EN_PROCESO);
                    TempData["SuccessMessage"] = "Tarea elegida correctamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo elegir la tarea";
                }

                var tareasDelProyecto = await _tareaBL.GetTareasByProyectoIdAsync(tarea.IdProyecto);

                return View("Index", tareasDelProyecto);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado: "+ ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}