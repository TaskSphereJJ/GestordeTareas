//using GestordeTaras.EN;
//using GestordeTareas.BL;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace GestordeTareas.UI.Controllers
//{
//    public class ImagenTareaController : Controller
//    {
//        private readonly BL.ImagenesPruebasBL _imagenTareaBL;

//        public ImagenTareaController()
//        {
//            _imagenTareaBL = new ImagenesPruebasBL();
//        }

//        // GET: ImagenTareaController
//        public async Task<ActionResult> Index()
//        {
//            var imagennesPruebas = await _imagenTareaBL.GetAllAsync();
//            return View(imagennesPruebas);
//        }

//        // GET: ImagenTareaController/Details/5
//        public async Task<ActionResult> Details(int id)
//        {
//            var imagenesPruebas = await _imagenTareaBL.GetById(new GestordeTaras.EN.ImagenesPrueba { Id = id });
//            return View(imagenesPruebas);
//        }

//        // GET: ImagenTareaController/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: ImagenTareaController/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create(GestordeTaras.EN.ImagenesPrueba imagenesPruebas)
//        {
//            try
//            {
//                await _imagenTareaBL.CreateAsync(imagenesPruebas);
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: ImagenTareaController/Edit/5
//        public async Task<ActionResult> Edit(int id)
//        {
//            var imagenesPruebas = await _imagenTareaBL.GetById(new GestordeTaras.EN.ImagenesPrueba { Id = id });
//            return View(imagenesPruebas);
//        }

//        // POST: ImagenTareaController/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit(int id, GestordeTaras.EN.ImagenesPrueba imagenesPruebas)
//        {
//            try
//            {
//                await _imagenTareaBL.UpdateAsync(imagenesPruebas);
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: ImagenTareaController/Delete/5
//        public async Task<ActionResult> Delete(int id)
//        {
//            var imagenesPruebas = await _imagenTareaBL.GetById(new GestordeTaras.EN.ImagenesPrueba { Id = id });
//            return View(imagenesPruebas);
//        }

//        // POST: ImagenTareaController/Delete/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Delete(int id, GestordeTaras.EN.ImagenesPrueba imagenesPruebas)
//        {
//            try
//            {
//                await _imagenTareaBL.DeleteAsync(imagenesPruebas);
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }
//    }
//}