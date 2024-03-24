using GestordeTaras.EN;
using GestordeTareas.BL;
using GestordeTareas.DAL;
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
        public async Task<ActionResult> Index()
        {
            List<Tarea> Lista = await _tareaBL.GetAllAsync();

            return View(Lista);
        }

        // GET: TareaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            return PartialView("Details", tarea);
        }

        // GET: TareaController/Create
        public async Task<ActionResult> Create()
        {
            await LoadDropDownListsAsync(); //Se llama al método y se espera que cargue
            return PartialView("Create");
        }

        // POST: TareaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tarea tarea)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int estadoPendienteId = await EstadoTareaDAL.GetEstadoPendienteIdAsync();
                    tarea.IdEstadoTarea = estadoPendienteId;
                    await LoadDropDownListsAsync();

                    // Lógica para crear la nueva tarea
                    await _tareaBL.CreateAsync(tarea);
                    return RedirectToAction(nameof(Index));
                }
                await LoadDropDownListsAsync(); // Cargar listas en caso de modelo no válido
                return PartialView("Create", tarea);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                await LoadDropDownListsAsync();
                return PartialView("Create", tarea);
            }
        }

        //MÉTODO PARA CARGAR LISTAS DESPLEGABLES SELECCIONABLES 
        private async Task LoadDropDownListsAsync()
        {
            // Obtener todos los datos de cada una disponibles
            var categorias = await _categoriaBL.GetAllAsync();
            var prioridades = await _prioridadBL.GetAllAsync();
            //var estadosTarea = await _estadoTareaBL.GetAllAsync();
            var proyectos = await _proyectoBL.GetAllAsync();

            // Se crean SelectList para cada entidad con las propiedades Id como valor y Nombre como texto visible
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");
            ViewBag.Prioridades = new SelectList(prioridades, "Id", "Nombre");
            //ViewBag.EstadosTarea = new SelectList(estadosTarea, "Id", "Nombre");
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
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(tarea);
            }
        }
    }
}
