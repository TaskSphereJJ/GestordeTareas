//using GestordeTaras.EN;
//using GestordeTareas.BL;
//using GestordeTareas.UI.Helpers;  
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.IO;
//using System.Threading.Tasks;

//namespace GestordeTareas.UI.Controllers
//{
//    public class TareaFinalizadaController : Controller
//    {
//        private readonly TareaFinalizadaBL _tareaFinalizadaBL;
//        private readonly ImagenesPruebaBL _imagenesPruebaBL;

//        public TareaFinalizadaController()
//        {
//            _tareaFinalizadaBL = new TareaFinalizadaBL();
//            _imagenesPruebaBL = new ImagenesPruebaBL();
//        }

//        public async Task<IActionResult> Index()
//        {
//            var tareasFinalizadas = await _tareaFinalizadaBL.GetAllAsync();
//            var tareasFiltradas = tareasFinalizadas.Where(t => t.ElegirTarea.EstadoTarea.Id == 3).ToList();
//            return View(tareasFiltradas);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CrearTareaFinalizada(TareaFinalizada tareaFinalizada, IFormFile archivoImagen)
//        {
//            if (archivoImagen != null && archivoImagen.Length > 0)
//            {
//                // Subir la imagen a Firebase
//                string imagenUrl = await ImageHelper.SubirArchivo(archivoImagen.OpenReadStream(), archivoImagen.FileName);

//                if (!string.IsNullOrEmpty(imagenUrl))
//                {
//                    // Crear tarea finalizada
//                    var tareaResult = await _tareaFinalizadaBL.CreateAsync(tareaFinalizada);

//                    if (tareaResult > 0)
//                    {
//                        // Crear el objeto ImagenesPrueba y asociarlo con la tarea finalizada
//                        ImagenesPrueba imagenesPrueba = new ImagenesPrueba
//                        {
//                            Imagen = imagenUrl,
//                            IdTareaFinalizada = tareaFinalizada.Id
//                        };

//                        // Guardar la imagen en la base de datos
//                        var imagenResult = await _imagenesPruebaBL.CreateAsync(imagenesPrueba);

//                        if (imagenResult > 0)
//                        {
//                            // Redirigir o devolver éxito si todo se guardó correctamente
//                            return RedirectToAction("Index");  // O la vista que desees
//                        }
//                        else
//                        {
//                            ModelState.AddModelError("", "No se pudo guardar la imagen en la base de datos.");
//                        }
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "No se pudo crear la tarea finalizada.");
//                    }
//                }
//                else
//                {
//                    ModelState.AddModelError("", "No se pudo subir la imagen a Firebase.");
//                }
//            }
//            else
//            {
//                ModelState.AddModelError("", "Por favor, seleccione una imagen.");
//            }

//            return View(tareaFinalizada); 
//        }
//    }
//}
