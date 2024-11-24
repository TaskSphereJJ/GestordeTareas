//using GestordeTaras.EN;
//using GestordeTareas.BL;
//using GestordeTareas.UI.Helpers;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace GestordeTareas.UI.Controllers
//{
//    public class TareaFinalizadaController : Controller
//    {
//        private readonly TareaFinalizadaBL _tareaFinalizadaBL;

//        public TareaFinalizadaController(TareaFinalizadaBL tareaFinalizadaBL)
//        {
//            _tareaFinalizadaBL = tareaFinalizadaBL;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ImagenTareaFinalizada(int idTarea, string comentario, List<IFormFile> formFiles)
//        {
//            try
//            {
//                if (formFiles == null || formFiles.Count == 0)
//                {
//                    ModelState.AddModelError("", "Debe proporcionar al menos una imagen.");
//                    return View();
//                }

//                // Subir las imágenes a Firebase y obtener las rutas
//                var rutasImagenes = new List<string>();
//                foreach (var file in formFiles)
//                {
//                    if (file.Length > 0)
//                    {
//                        using var stream = file.OpenReadStream();
//                        string ruta = await ImageHelper.SubirArchivo(stream, file.FileName);
//                        rutasImagenes.Add(ruta);
//                    }
//                }

//                // Crear objeto de tarea finalizada
//                var tareaFinalizada = new TareaFinalizada
//                {
//                    IdTarea = idTarea,
//                    FechaFinalizacion = DateTime.Now,
//                    Comentarios = comentario,
//                    IdEstadoTarea = 3 // Estado finalizado
//                };

//                // Llamar a la lógica de negocio para guardar la tarea y las imágenes
//                int idTareaFinalizada = await _tareaFinalizadaBL.CrearTareaFinalizadaConImagenesAsync(tareaFinalizada, rutasImagenes);

//                TempData["Success"] = "La tarea finalizada se guardó exitosamente.";
//                return RedirectToAction("Index");
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = $"Ocurrió un error al procesar la solicitud: {ex.Message}";
//                return View();
//            }
//        }
//    }
//}
