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
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using GestordeTareas.UI.Helpers;

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsuarioController : Controller
    {
        UsuarioBL _usuarioBL = new UsuarioBL();
        CargoBL cargoBL = new CargoBL();
        private readonly CargoBL _cargoBL;
        private readonly IEmailService _emailService;

        public UsuarioController(IEmailService emailService)
        {
            _cargoBL = new CargoBL();
            _emailService = emailService;

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
        public async Task<ActionResult> Index(string query = "", string filter = "NombreUsuario", int top = 10)
        {
            var user = new Usuario();
            // Número de registros a mostrar
            if (top <= 0)
                top = 10; // valor predeterminado
            user.Top_Aux = top;

            List<Usuario> Lista = await _usuarioBL.SearchIncludeRoleAsync(user, query, filter);
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
                await LoadDropDownListsAsync();
                // Obtener el nombre de usuario
                string nombreUsuario = User.Identity.Name;

                // Pasar el nombre de usuario a la vista
                ViewBag.NombreUsuario = nombreUsuario;
                var users = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = User.Identity.Name, Top_Aux = 1 });
                var actualUser = users.FirstOrDefault();

                //ESTO ES PARA OBTENER EL NOMBRE Y APELLIDO Y PONERLO EN EL LAYOUT 
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
                ViewBag.ErrorMessage = "Ocurrió un error al cargar la información del usuario";
                return View(); // Puedes redirigir a una vista de error específica si lo deseas
            }
        }


        // GET: UsuarioController/Details/5
        public async Task<ActionResult> DetailsPartial(int id)
        {
            var user = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            user.Cargo = await cargoBL.GetById(new Cargo { Id = user.IdCargo });
            return PartialView("Details", user);
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
        public async Task<ActionResult> Create(Usuario usuario, IFormFile fotoPerfil)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario.Pass) || usuario.Pass.Length < 8)
                {
                    TempData["ErrorMessage"] = "La contraseña debe tener al menos 8 caracteres";
                    return View(usuario);
                }

                usuario.Status = (byte)User_Status.ACTIVO; // Valor predeterminado al crear usuario

                // Manejar la foto de perfil usando el nuevo método
                if (fotoPerfil != null && fotoPerfil.Length > 0)
                {
                    usuario.FotoPerfil = await ImageHelper.SubirArchivo(fotoPerfil.OpenReadStream(), fotoPerfil.FileName);
                }

                if (User.IsInRole("Administrador"))
                  {
                      int createresult = await _usuarioBL.Create(usuario);
                      TempData["SuccessMessage"] = "Usuario creado correctamente";
                      return RedirectToAction(nameof(Index));
                  }

                // Si no es administrador, asigna un rol predeterminado
                var cargoColaboradorId = await CargoDAL.GetCargoColaboradorIdAsync(); // Método para obtener el ID del cargo predeterminado
                usuario.IdCargo = cargoColaboradorId;

                int result = await _usuarioBL.Create(usuario);
                TempData["SuccessMessage"] = "Usuario creado correctamente. Por favor, inicia sesión";
                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
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
                // Obtener el usuario existente de la base de datos
                var existingUser = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
                if (existingUser == null)
                {
                    TempData["ErrorMessage"] = "El usuario no fue encontrado";
                    return Json(new { success = false, message = "Usuario no encontrado" });
                }

                // Mantener la contraseña existente
                usuario.Pass = existingUser.Pass;

                // Llamar al método de actualización
                int result = await _usuarioBL.Update(usuario);
                TempData["SuccessMessage"] = "Usuario actualizado correctamente.";
                return Json(new { success = true, message = "Usuario actualizado correctamente" });
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hubo un problema al actualizar el usuario";
                return Json(new { success = false, message = "Hubo un problema al actualizar el usuario: " + ex.Message });
            }

        }


        //MÉTODO PARA PODER EDITAR LA INFORMACION PARA EL USUARIO LOGUEADO EN SU PERFIL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOwn(Usuario usuario, string currentPassword, IFormFile fotoPerfil)
        {
            try
            {
                var existingUser = await _usuarioBL.GetByIdAsync(usuario);
                if (existingUser == null)
                {
                    TempData["ErrorMessage"] = "El usuario no fue encontrado";
                    return RedirectToAction("Perfil");
                }

                // Si el usuario proporciona una nueva contraseña, verifica la contraseña actual
                if (!string.IsNullOrEmpty(usuario.Pass))
                {

                    // Verifica si la contraseña actual coincide con la almacenada
                    if (UsuarioDAL.HashMD5(currentPassword) != existingUser.Pass)
                    {
                        TempData["ErrorMessage"] = "La contraseña actual es incorrecta";
                        return RedirectToAction("Perfil");
                    }

                    // Verificar que la nueva contraseña y la confirmación coincidan
                    if (usuario.Pass != usuario.ConfirmarPass)
                    {
                        TempData["ErrorMessage"] = "La nueva contraseña y la confirmación no coinciden";
                        return RedirectToAction("Perfil");
                    }

                    existingUser.Pass = UsuarioDAL.HashMD5(usuario.Pass);
                }

                // Actualizar los campos comunes para todos los usuarios
                existingUser.Nombre = usuario.Nombre;
                existingUser.Apellido = usuario.Apellido;
                existingUser.Telefono = usuario.Telefono;
                existingUser.FechaNacimiento = usuario.FechaNacimiento;
                existingUser.NombreUsuario = usuario.NombreUsuario;

                // Manejar la foto de perfil usando el nuevo método
                if (fotoPerfil != null && fotoPerfil.Length > 0)
                {
                    existingUser.FotoPerfil = await ImageHelper.SubirArchivo(fotoPerfil.OpenReadStream(), fotoPerfil.FileName);
                }


                // Permitir que el administrador cambie los campos adicionales solo si es su propio perfil
                if (User.IsInRole("Administrador") && existingUser.Id == usuario.Id)
                {
                    existingUser.Cargo = usuario.Cargo;
                    existingUser.Status = usuario.Status;
                }

                // Actualiza los claims del usuario en el contexto actual
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.GivenName));
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Surname));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, existingUser.Nombre)); 
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, existingUser.Apellido));
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("FotoPerfil")); // Eliminar el claim viejo si existe
                claimsIdentity.AddClaim(new Claim("FotoPerfil", existingUser.FotoPerfil)); // Agregar el nuevo claim de foto de perfil


                var principal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                // Actualiza el usuario en la base de datos
                await _usuarioBL.Update(existingUser);
                TempData["SuccessMessage"] = "Perfil actualizado correctamente";
                return RedirectToAction("Perfil");
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el perfil: {ex.Message}";
                return RedirectToAction("Perfil");
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
                TempData["SuccessMessage"] = "Usuario eliminado correctamente";
                return Json(new { success = true, message = "Usuario eliminado correctamente" });

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hubo un problema al eliminar el usuario: " + ex.Message;
                return Json(new { success = false, message = "Hubo un problema al eliminar el usuario: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOwn()
        {

            try
            {
                // Verifica si el usuario está autenticado
                if (!User.Identity.IsAuthenticated)
                {
                    TempData["ErrorMessage"] = "Usuario no autenticado";
                    return RedirectToAction("Login", "Usuario");
                }

                var nombreUsuario = User.Identity.Name;

                // Se verifica si userId es null o vacío
                if (string.IsNullOrEmpty(nombreUsuario))
                {
                    TempData["ErrorMessage"] = "No se pudo encontrar el usuario";
                    return RedirectToAction("Perfil");
                }

                // Crear el objeto usuario con el ID
                var usuario = new Usuario { NombreUsuario = nombreUsuario };

                // Obtener el usuario de la base de datos
                var usuarioDB = await _usuarioBL.GetByNombreUsuarioAsync(usuario);

                // Verificar que el usuario exista
                if (usuarioDB == null)
                {
                    TempData["ErrorMessage"] = "El usuario no existe";
                    return RedirectToAction("Perfil");
                }

                // Eliminar el usuario
                int result = await _usuarioBL.Delete(usuarioDB);

                // Verificar si la eliminación fue exitosa
                if (result > 0)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    TempData["SuccessMessage"] = "Cuenta eliminada correctamente";
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar la cuenta";
                    return RedirectToAction("Perfil");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                TempData["ErrorMessage"] = "Hubo un problema al eliminar la cuenta: " + ex.Message;
                return RedirectToAction("Perfil");
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
                // Verificamos si el nombre de usuario existe usando GetByNombreUsuarioAsync
                var userDb = await _usuarioBL.GetByNombreUsuarioAsync(user);
                if (userDb == null)
                {
                    // Si el usuario no existe
                    TempData["ErrorMessage"] = "El correo electrónico ingresado no existe";
                    return View(new Usuario { NombreUsuario = user.NombreUsuario });
                }

                // Verificar el estado del usuario
                if (userDb.Status != (byte)User_Status.ACTIVO)
                {
                    TempData["ErrorMessage"] = "Tu cuenta está inactiva. Por favor, contacta al administrador.";
                    return View(new Usuario { NombreUsuario = user.NombreUsuario });
                }

                // Si el usuario existe, verificamos si la contraseña es correcta
                if (userDb.Pass != UsuarioDAL.HashMD5(user.Pass))  // Usamos HashMD5 para comparar contraseñas cifradas
                {
                    // Si la contraseña es incorrecta
                    TempData["ErrorMessage"] = "La contraseña es incorrecta";
                    return View(new Usuario { NombreUsuario = user.NombreUsuario });
                }

                    // Verifica si la propiedad FotoPerfil tiene un valor
                    var fotoPerfil = string.IsNullOrEmpty(userDb.FotoPerfil) ? "/img/usuario.png" : userDb.FotoPerfil;

                    userDb.Cargo = await cargoBL.GetById(new Cargo { Id = userDb.IdCargo });
                    var claims = new[] {
                    new Claim(ClaimTypes.Name, userDb.NombreUsuario),
                    new Claim(ClaimTypes.Role, userDb.Cargo.Nombre),
                    new Claim(ClaimTypes.GivenName, userDb.Nombre),   
                    new Claim(ClaimTypes.Surname, userDb.Apellido),
                    new Claim(ClaimTypes.NameIdentifier, userDb.Id.ToString()),
                    new Claim("FotoPerfil", fotoPerfil)
            };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    // Verificar si hay un token y una decisión almacenados en TempData
                    if (TempData.ContainsKey("Token") && TempData.ContainsKey("Decision"))
                    {
                        string token = TempData["Token"].ToString();
                        string decision = TempData["Decision"].ToString();

                        // Limpiar TempData después de redirigir
                        TempData.Remove("Token");
                        TempData.Remove("Decision");

                        return RedirectToAction("AceptarInvitacion", "Proyecto", new { token = token, decision = decision });
                    }

                    // Si no hay token, redirigir al returnUrl o a la vista predeterminada
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                // Mensaje de error en caso de excepción
                TempData["ErrorMessage"] = ex.Message;
                ViewBag.Url = returnUrl;
                return View(new Usuario { NombreUsuario = user.NombreUsuario });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Usuario");

        }

        // GET: UsuarioController/ForgotPassword
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //MÉTODO PARA ENVIAR EL CORREO CON EL CODIGO DE VERIFICACION
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SolicitarRestablecimiento(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                return Json(new { success = false, message = "El correo electrónico no puede estar vacío" });

            }

            try
            {
                // Busca al usuario por su NombreUsuario que corresponde al correo en este caso
                var usuario = await _usuarioBL.SearchAsync(new Usuario { NombreUsuario = nombreUsuario });
                var usuarioEncontrado = usuario.FirstOrDefault();

                if (usuarioEncontrado != null)
                {
                    //se almacena el usuario en tempdata
                    TempData["IdUsuario"] = usuarioEncontrado.Id;
                    // Genera el código de restablecimiento
                    int codigo = await _usuarioBL.GenerarCodigoRestablecimientoAsync(usuarioEncontrado);

                    if (codigo == 0)
                    {
                        return Json(new { success = false, message = "Error al generar el código de restablecimeiento" });

                    }

                    try
                    {
                        // Envía el código de restablecimiento al usuario
                        await _emailService.SendPasswordResetAsync(usuarioEncontrado.NombreUsuario, codigo);
                        return Json(new { success = true, message = "Código de verificación enviado" });


                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = $"Error al enviar el correo: {ex.Message}" });

                    }
                }
                else
                {
                    // Si el usuario no existe, muestra un mensaje de error
                    return Json(new { success = false, message = "El correo electrónico ingresado no existe en nuestros registros" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hubo un problema al procesar tu solicitud: {ex.Message}" });

            }
        }


        //MÉTODO PARA VERIFICAR SI EL CODIGO COINCIDE CON EL ENVIADO
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ValidarCodigo(string codigo)
        {
            var idUsuario = TempData["IdUsuario"] as int?;
            TempData["IdUsuario"] = idUsuario;

            if (idUsuario == null)
            {
                return Json(new { success = false, message = "No se encontró información del usuario. Vuelve a solicitar el código" });

            }

            if (string.IsNullOrWhiteSpace(codigo))
            {
                return Json(new { success = false, message = "Por favor, ingresa el código de verificación" });

            }

            // Verifica si el código ingresado es correcto
            bool esCodigoValido = await _usuarioBL.ValidarCodigoRestablecimientoAsync(idUsuario.Value, codigo);

            if (!esCodigoValido)
            {
                // Si el código no es válido o ha expirado
                return Json(new { success = false, message = "El código ingresado es incorrecto o ha expirado" });

            }

            // Guarda el código en TempData para ser usado en el siguiente metodo
            TempData["CodigoRestablecimiento"] = codigo;

            return Json(new { success = true, message = "El código es válido. Ahora puedes restablecer tu contraseña" });

        }


        //MÉTODO PARA CAMBIAR LA CONTRASEÑA UNA VEZ HA SIDO VERIFICADO EL CODIGO
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RestablecerContrasena(string nuevaContrasena, string confirmarContrasena)
        {
            var idUsuario = TempData["IdUsuario"] as int?;
            TempData["IdUsuario"] = idUsuario;

            if (idUsuario == null)
            {
                return Json(new { success = false, message = "No se encontró información del usuario. Vuelve a solicitar el código" });

            }

            if (string.IsNullOrWhiteSpace(nuevaContrasena) || string.IsNullOrWhiteSpace(confirmarContrasena))
            {
                return Json(new { success = false, message = "Por favor, ingresa ambas contraseñas" });

            }

            if (nuevaContrasena != confirmarContrasena)
            {
                return Json(new { success = false, message = "Las contraseñas no coinciden. Por favor, intenta nuevamente" });

            }

            if (nuevaContrasena.Length < 8)  
            {
                return Json(new { success = false, message = "La contraseña debe tener al menos 8 caracteres" });

            }

            try
            {
                // Recupera el código de verificación desde TempData (si ya se validó previamente)
                var codigoGuardado = TempData["CodigoRestablecimiento"] as string;

                if (string.IsNullOrWhiteSpace(codigoGuardado))
                {
                    return Json(new { success = false, message = "El código de verificación no es válido o ha expirado" });

                }

                // Si la nueva contraseña se ha ingresado, intenta restablecerla
                var resultado = await _usuarioBL.RestablecerContrasenaAsync(idUsuario.Value, codigoGuardado, nuevaContrasena);

                if (resultado > 0)
                {
                    // Si la contraseña fue actualizada correctamente
                    return Json(new { success = true, message = "Tu contraseña ha sido actualizada exitosamente" });

                }
                else
                {
                    // Si hubo algún problema al restablecer la contraseña
                    return Json(new { success = false, message = "No se pudo restablecer la contraseña. Intenta nuevamente" });

                }
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y muestra un mensaje de error
                return Json(new { success = false, message = $"Hubo un problema al procesar tu solicitud: {ex.Message}" });

            }
        }

    }

}