using GestordeTaras.EN;
using GestordeTareas.BL;
using GestordeTareas.DAL;
using Humanizer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public TareaController()
        {
            _tareaBL = new TareaBL();
            _categoriaBL = new CategoriaBL();
            _prioridadBL = new PrioridadBL();
            _estadoTareaBL = new EstadoTareaBL();
            _proyectoBL = new ProyectoBL();
        }

        // GET: TareaController
        public async Task<ActionResult> Index(int proyectoId)
        {
            //List<Tarea> Lista = await _tareaBL.GetAllAsync();

            //return View(Lista);

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
            try
            {
                // Asignar el ID del proyecto a la tarea
                tarea.IdProyecto = idProyecto;
                tarea.FechaCreacion = DateTime.Now;
                int estadoPendienteId = await EstadoTareaDAL.GetEstadoPendienteIdAsync();
                tarea.IdEstadoTarea = estadoPendienteId;

                int result = await _tareaBL.CreateAsync(tarea);
                return RedirectToAction(nameof(Index), new {id = idProyecto});
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
            try
            {
                int result = await _tareaBL.UpdateAsync(tarea);
                return RedirectToAction(nameof(Index), new { id = tarea.IdProyecto});
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
            try
            {
                await _tareaBL.DeleteAsync(tarea);
                return RedirectToAction(nameof(Index), new {id = tarea.IdProyecto});
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(tarea);
            }
        }

        //ACTUALIZAR ESTADO
        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> ActualizarEstadoTarea(Tarea tarea)
        {
            try
            {
                await _tareaBL.UpdateAsync(tarea);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }



    }
}