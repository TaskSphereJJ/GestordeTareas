using GestordeTaras.EN;
using GestordeTareas.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    public class TareaController : Controller
    {
        private readonly TareaBL _tareaBL;

        public TareaController()
        {
            _tareaBL = new TareaBL(); // Inicializamos la capa de negocio
        }

        //get tareasControllers
        public async Task<ActionResult> Index() 
        {
            var tareas = await _tareaBL.GetAllAsync();
            return View(tareas);
        }

        // GET: TAREAController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var tarea = await _tareaBL.GetById(new Tarea { Id = id });
            return View(tarea);
        }

        // GET: TareaController/Crear
        public ActionResult Create()
        {
            return View();
        }
    }

}

