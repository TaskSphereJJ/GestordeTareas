//using GestordeTaras.EN;
//using GestordeTareas.BL;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace GestordeTareas.UI.Controllers
//{
//    public class GrupoTrabajoController : Controller
//    {
//        private readonly GrupoTrabajoBL _grupoTrabajoBL;

//        public GrupoTrabajoController(GrupoTrabajoBL grupoTrabajoBL)
//        {
//            _grupoTrabajoBL = grupoTrabajoBL;
//        }

//        // GET: GrupoTrabajo
//        public async Task<ActionResult> Index()
//        {
//            List<GrupoTrabajo> lista = await _grupoTrabajoBL.GetAllAsync();
//            return View(lista);
//        }

//        // GET: GrupoTrabajo/Details/5
//        public async Task<ActionResult> Details(int id)
//        {
//            var grupoTrabajo = await _grupoTrabajoBL.GetByIdAsync(id);
//            return View(grupoTrabajo);
//        }

//        // GET: GrupoTrabajo/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: GrupoTrabajo/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create(GrupoTrabajo grupoTrabajo)
//        {
//            try
//            {
//                int result = await _grupoTrabajoBL.CreateAsync(grupoTrabajo);
//                return RedirectToAction(nameof(Index));
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = ex.Message;
//                return View(grupoTrabajo);
//            }
//        }

//        // GET: GrupoTrabajo/RegistrarCodigoAcceso
//        public ActionResult RegistrarCodigoAcceso()
//        {
//            return View();
//        }

//        // POST: GrupoTrabajo/RegistrarCodigoAcceso
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> RegistrarCodigoAcceso(string codigoAcceso)
//        {
//            try
//            {
//                bool codigoValido = await _grupoTrabajoBL.ValidarCodigoAccesoAsync(1, codigoAcceso); // Supongo que aquí deberías pasar el ID del proyecto correspondiente
//                if (codigoValido)
//                {
//                    // Agregar lógica para unir al usuario al grupo de trabajo como colaborador
//                    var usuarioId = 1; // Aquí deberías obtener el ID del usuario actual
//                    await _grupoTrabajoBL.UnirUsuarioGrupoTrabajoAsync(usuarioId);

//                    // Obtener la lista de miembros del grupo de trabajo
//                    var listaMiembros = await _grupoTrabajoBL.GetMiembrosGrupoTrabajoAsync(); // Implementa este método en la BL para obtener la lista de miembros

//                    // Mostrar la vista con la lista de miembros del grupo
//                    return View("ListaMiembros", listaMiembros);
//                }
//                else
//                {
//                    ViewBag.Error = "El código de acceso no es válido.";
//                    return View();
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = ex.Message;
//                return View();
//            }
//        }


//    }

//}
