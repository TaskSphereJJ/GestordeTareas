using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestordeTareas.BL;
using GestordeTaras.EN;

namespace GestordeTareas.UI.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioBL _usuarioBL;
        public UsuarioController() 
        {
            _usuarioBL = new UsuarioBL();
        }
        // GET: UsuarioController
        public async Task<ActionResult> Index()
        {
            List<Usuario> Lista = await _usuarioBL.GetAllAsync();
            return View(Lista);
        }

        // GET: UsuarioController/Details/5
        public async Task<ActionResult> Usuario(int id)
        {
            // Instancia de modelo para representar un usuario
            Usuario model_usuario = new Usuario();

            // ViewBag para indicar que se está creando un nuevo usuario
            ViewBag.Accion = "Nuevo Usuario";

            // Si el idUsuario no es 0, significa que se está editando un usuario existente
            if (id != 0)
            {
                // Llama al método Obtener(idUsuario) del servicio para obtener información del usuario con el ID proporcionado
                model_usuario = await _usuarioBL.GetByIdAsync(new Usuario { Id = id});

                // Indica que se está editando un usuario
                ViewBag.Accion = "Editar Usuario";
            }

            // Devuelve la vista "Usuario" con el modelo(info) de usuario correspondiente
            return View(model_usuario);
            
        }


        // Acción del controlador para guardar cambios en un usuario
        [HttpPost]
         
        public async Task<IActionResult> GuardarCambios(Usuario ob_Usuario)
        {
            int resultado;

            // Verifica si el userId del usuario es igual a cero, lo cual indica que es un nuevo usuario
            if (ob_Usuario.Id == 0)
                // Llama al método Guardar(ob_Usuario) del servicio para guardar un nuevo usuario
                resultado = await _usuarioBL.Create(ob_Usuario);
            else
                // Llama al método Editar(ob_Usuario) del servicio para editar un usuario existente
                resultado = await _usuarioBL.Update(ob_Usuario);

            // Verifica el resultado y redirige a la página de inicio si es exitoso
            if (resultado > 0)
                return RedirectToAction("Index");
            else
                // Si la operación falla o no se obtiene un ID válido, devuelve una respuesta sin contenido
                return NoContent();
        }




        // Acción del controlador para eliminar un usuario
        [HttpGet]
        public async Task<IActionResult> Eliminar(Usuario usuario)
        {
            // Llama al método Eliminar(idUsuario) del servicio para eliminar un usuario
            var respuesta = await _usuarioBL.Delete(usuario);

            if (respuesta > 0)
                // Verifica la respuesta del servicio y redirige a la página de inicio si es exitosa
                return RedirectToAction("Index");
            else
                // Si la operación falla o no se eliminan filas, puedes manejarlo según tus necesidades
                return NoContent();
        }
    }
}
