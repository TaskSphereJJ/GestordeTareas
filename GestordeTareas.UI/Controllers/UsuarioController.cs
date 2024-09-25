using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestordeTareas.BL;
using GestordeTaras.EN;
using GestordeTareas.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsuarioController : Controller
    {
        UsuarioBL _usuarioBL = new UsuarioBL();
        CargoBL cargoBL = new CargoBL();
        private readonly CargoBL _cargoBL;

        public UsuarioController()
        {
            _cargoBL = new CargoBL();

        }

        //MÉTODO PARA CARGAR LISTAS DESPLEGABLES SELECCIONABLES 
        private async Task LoadDropDownListsAsync()
        {
            // Obtener todos los datos 
            var cargos = await _cargoBL.GetAllAsync();

            // Se crean SelectList para cada entidad con las propiedades Id como valor y Nombre como texto visible
            ViewBag.Cargos = new SelectList(cargos, "Id", "Nombre");
        }


        // GET: UsuarioController
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Index(Usuario user = null)
        {
            //List<Usuarios> Lista = await _usuarioBL.GetAllAsync();
            //return View(Lista);
            if (user == null)
                user = new Usuario();
            if (user.Top_Aux == 0)
                user.Top_Aux = 10; // setear la cantidad de registros a mostrar predeterminadamente
            else if (user.Top_Aux == -1)
                user.Top_Aux = 0;

            List<Usuario> Lista = await _usuarioBL.SearchIncludeRoleAsync(user);
            Lista = Lista.OrderBy(u => u.Id).ToList();
            ViewBag.Top = user.Top_Aux;
            ViewBag.Roles = await cargoBL.GetAllAsync();
            return View(Lista);
        }

        [Authorize]
        public async Task<ActionResult> Perfil()
        {
            try
            {
                // Obtener el nombre de usuario
                string nombreUsuario = User.Identity.Name;

                // Pasar el nombre de usuario a la vista
                ViewBag.NombreUsuario = nombreUsuario;
                var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
                var actualUser = users.FirstOrDefault();

                //ESTO ES PARA OBTENER EL NOMBRE Y APELLIDO Y PONERLO EN EL LAYOUT 
                // Obtener el nombre y apellido del usuario
                string nombre = actualUser.Nombre;
                string apellido = actualUser.Apellido;

                // Pasar el nombre y apellido del usuario a la vista
                ViewBag.NombreUsuario = $"{nombre} {apellido}";
                if (actualUser == null)
                {
                    // Manejar el caso en que no se encuentra el usuario autenticado
                    return NotFound();
                }

                // Utiliza el ID del usuario autenticado para buscar sus datos específicos
                int userId = actualUser.Id;
                Usuario user = await _usuarioBL.GetByIdAsync(new Usuario { Id = userId });

                // Pasa los datos del usuario a la vista
                return View(user);
            }
            catch (Exception ex)
            {
                // Manejar la excepción de forma adecuada, por ejemplo, registrándola o mostrando un mensaje de error
                ViewBag.ErrorMessage = "Ocurrió un error al cargar la información del usuario.";
                return View(); // Puedes redirigir a una vista de error específica si lo deseas
            }
        }


        // GET: UsuarioController/Details/5
        public async Task<ActionResult> DetailsPartial(int id)
        {
            var user = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            user.Cargo = await cargoBL.GetById(new Cargo { Id = user.IdCargo });
            return View("Details",user);
        }

        // GET: UsuarioController/Create
        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            await LoadDropDownListsAsync();
            return View();
        }

        // POST: UsuarioController/Create
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            try
            {

                usuario.Status = (byte)User_Status.ACTIVO; // Valor predeterminado al crear usuario

                if (User.IsInRole("Administrador"))
                {
                    // Si es administrador, puedes permitir que el rol sea seleccionado
                    if (ModelState.IsValid)
                    {
                        int createresult = await _usuarioBL.Create(usuario);
                        TempData["SuccessMessage"] = "Usuario creado correctamente. Por favor, inicie sesión.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // Si no es administrador, asigna un rol predeterminado
                // Obtén el ID del cargo predeterminado para colaboradores
                var cargoColaboradorId = await CargoDAL.GetCargoColaboradorIdAsync(); // Método para obtener el ID del cargo predeterminado
                usuario.IdCargo = cargoColaboradorId;

                int result = await _usuarioBL.Create(usuario);
                TempData["SuccessMessage"] = "Usuario creado correctamente. Por favor, inicie sesión.";
                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                await LoadDropDownListsAsync();
                return View(usuario);
            }
        }


        // GET: UsuarioController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // Obtener el usuario por ID
            var usuario = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });

            // Verificar si el usuario fue encontrado
            if (usuario == null)
            {
                return NotFound(); // Devolver un error 404 si el usuario no se encuentra
            }

            // Cargar listas desplegables necesarias para la vista
            await LoadDropDownListsAsync();

            // Devolver la vista parcial "Edit" con el usuario
            return PartialView("Edit", usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int id, Usuario usuario)
        {
            try
            {
                int result = await _usuarioBL.Update(usuario);
                return Json(new { success = true, message = "Usuario actualizado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al actualizar el perfil: {ex.Message}" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOwn(Usuario usuario)
        {
            try
            {
                await LoadDropDownListsAsync();

                var existingUser = await _usuarioBL.GetByIdAsync(usuario);
                if (existingUser == null)
                {
                    return Json(new { success = false, message = "El usuario no fue encontrado." });
                }

                // Actualiza los campos comunes para todos los usuarios
                existingUser.Nombre = usuario.Nombre;
                existingUser.Apellido = usuario.Apellido;
                existingUser.Telefono = usuario.Telefono;
                existingUser.FechaNacimiento = usuario.FechaNacimiento;
                existingUser.NombreUsuario = usuario.NombreUsuario;

                // Solo actualiza la contraseña si se ha proporcionado
                if (!string.IsNullOrEmpty(usuario.Pass))
                {
                    existingUser.Pass = usuario.Pass; // Actualiza la contraseña
                }

                // Permitir que el administrador cambie los campos adicionales solo si es su propio perfil
                if (User.IsInRole("Administrador"))
                {
                    // Si está editando su propio perfil, puede cambiar más campos
                    if (existingUser.Id == usuario.Id)
                    {
                        existingUser.Cargo = usuario.Cargo;
                        existingUser.Status = usuario.Status;// Permitir cambio de cargo
                                                             // Aquí puedes añadir otras propiedades que quieras permitir editar
                    }
                }

                // Actualiza el usuario en la base de datos
                await _usuarioBL.Update(existingUser);
                return Json(new { success = true, message = "Perfil actualizado correctamente." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al actualizar el perfil: {ex.Message}" });
            }
        }

        // GET: UsuarioController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            usuario.Cargo = await cargoBL.GetById(new Cargo { Id = usuario.IdCargo });
            ViewBag.Error = "";
            return PartialView("Delete", usuario);
        }


        // POST: UsuarioController/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Usuario usuario)
        {
            try
            {
                int result = await _usuarioBL.Delete(usuario);
                return Json(new { success = true, message = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al eliminar el usuario: {ex.Message}" });
            }
        }

        // Método para eliminar la cuenta del usuario que está logueado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOwn()
        {
            // Obtener el ID del usuario que está actualmente logueado
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int usuarioId;

            // Intentar convertir el ID de usuario a un entero
            if (!int.TryParse(currentUserId, out usuarioId))
            {
                ViewBag.Error = "ID de usuario no válido.";
                return View("Perfil"); // Manejo de error si la conversión falla
            }

            // Obtener el usuario correspondiente al ID
            var usuario = await _usuarioBL.GetByIdAsync(new Usuario { Id = usuarioId });

            // Verificar que el usuario exista
            if (usuario == null)
            {
                // Manejar el caso donde el usuario no se encuentra
                ViewBag.Error = "El usuario no existe.";
                return View("Perfil"); // O redirigir a otra vista
            }

            try
            {
                // Eliminar el usuario
                int result = await _usuarioBL.Delete(usuario);
                return RedirectToAction("Login", "Usuario"); // Redirigir a logout o a una página de confirmación
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                ViewBag.Error = ex.Message;
                return View("Perfil"); // O redirigir a la vista del perfil
            }
        }

        // acción que muestra el formulario de inicio de sesión
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Url = returnUrl;
            ViewBag.Error = "";
            return View();
        }

        // acción que recibe los datos de inicio de sesión y ejecuta la autenticación
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Usuario user, string returnUrl = null)
        {
            try
            {
                var userDb = await _usuarioBL.LoginAsync(user);
                if (userDb != null && userDb.Id > 0 && userDb.NombreUsuario == user.NombreUsuario)
                {
                    userDb.Cargo = await cargoBL.GetById(new Cargo { Id = userDb.IdCargo });
                    var claims = new[] {
                new Claim(ClaimTypes.Name, userDb.NombreUsuario),
                new Claim(ClaimTypes.Role, userDb.Cargo.Nombre),
                new Claim("Nombre", userDb.Nombre),
                new Claim("Apellido", userDb.Apellido)
            };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                }
                else
                {
                    throw new Exception("Credenciales de usuario incorrectas");
                }

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Url = returnUrl;
                ViewBag.Error = ex.Message;
                return View(new Usuario { NombreUsuario = user.NombreUsuario });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Usuario");

        }

        //acción que muestra el formulario para cambiar contraseña
        public async Task<IActionResult> ChangePassword()
        {
            var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
            var actualUser = users.FirstOrDefault();
            ViewBag.Error = "";
            return View(actualUser);
        }

        //acción que recibe los datos de la nueva contraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(Usuario user, string oldPassword)
        {
            try
            {
                int result = await _usuarioBL.ChangePasswordAsync(user, oldPassword);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Usuario");

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
                var actualUser = users.FirstOrDefault();
                return View(actualUser);
            }
        }
    }

}