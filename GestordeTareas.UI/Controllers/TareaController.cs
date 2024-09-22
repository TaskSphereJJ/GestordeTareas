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

        public TareaController()
        {
            _tareaBL = new TareaBL();
            _categoriaBL = new CategoriaBL();
            _prioridadBL = new PrioridadBL();
            _estadoTareaBL = new EstadoTareaBL();
            _proyectoBL = new ProyectoBL();
            _usuarioBL = new UsuarioBL();
            _proyectoUsuarioBL = new ProyectoUsuarioBL();
        }

        // GET: TareaController
        public async Task<ActionResult> Index(int proyectoId)
        {
            if (!await VerificarAcceso(proyectoId))
            {
                TempData["ErrorMessage"] = "No tienes acceso a este proyecto.";
                return RedirectToAction("Index", "Proyecto"); // Redirigir a la vista de proyectos
            }

            // Aquí cargas las tareas asociadas al proyecto con el ID proporcionado
            var tareas = await _tareaBL.GetTareasByProyectoIdAsync(proyectoId);
            ViewBag.ProyectoId = proyectoId;
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
            var result = await _proyectoBL.GetById(proyecto);
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

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tarea tarea, int idProyecto)
        {
            if (!User.IsInRole("Administrador"))
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar cambios.";
                return View(tarea);
            }
            try
            {
                // Asignar el ID del proyecto a la tarea
                tarea.IdProyecto = idProyecto;
                tarea.FechaCreacion = DateTime.Now;
                int estadoPendienteId = await EstadoTareaDAL.GetEstadoPendienteIdAsync();
                tarea.IdEstadoTarea = estadoPendienteId;

                int result = await _tareaBL.CreateAsync(tarea);
                return RedirectToAction(nameof(Index), new { id = idProyecto });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                // Volver a cargar las listas desplegables u otros datos necesarios para la vista
                await LoadDropDownListsAsync();

                //  ViewBag.idProyecto = GetProyectoIdAsync(proyecto);
                // Devolver la vista parcial "Create" con la tarea y el ID de proyecto
                return PartialView("Create", new Tarea { IdProyecto = idProyecto });
                // return PartialView("Create", tarea);
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
        public async Task<ActionResult> Edit(int id, Tarea tarea)
        {
            if (!User.IsInRole("Administrador"))
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar cambios.";
                return View(tarea);
            }
            try
            {
                int result = await _tareaBL.UpdateAsync(tarea);
                return RedirectToAction(nameof(Index), new { id = tarea.IdProyecto });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                await LoadDropDownListsAsync();
                return View(tarea);
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
        public async Task<ActionResult> Delete(int id, Tarea tarea)
        {
            if (!User.IsInRole("Administrador"))
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar cambios.";
                return View(tarea);
            }
            try
            {
                await _tareaBL.DeleteAsync(tarea);
                return RedirectToAction(nameof(Index), new { id = tarea.IdProyecto });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(tarea);
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
                        if (estadoValido == null) return BadRequest("Estado no válido.");

                        tareaBD.IdEstadoTarea = model.IdEstadoTarea;
                        bdContexto.Update(tareaBD);
                        await bdContexto.SaveChangesAsync();
                        return Ok(new { nombreEstado = estadoValido.Nombre });
                    }
                    else
                    {
                        return NotFound("Tarea no encontrada.");
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

    }
}