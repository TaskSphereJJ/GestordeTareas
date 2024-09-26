﻿using Microsoft.AspNetCore.Http;
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
                await LoadDropDownListsAsync();
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
            return View("Details", user);
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
                        TempData["SuccessMessage"] = "Usuario creado correctamente.";
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
        public async Task<ActionResult> EditOwn(Usuario usuario, string currentPassword)
        {
            try
            {
                var existingUser = await _usuarioBL.GetByIdAsync(usuario);
                if (existingUser == null)
                {
                    TempData["ErrorMessage"] = "El usuario no fue encontrado.";
                    return RedirectToAction("Perfil");
                }

                // Si el usuario proporciona una nueva contraseña, verifica la contraseña actual
                if (!string.IsNullOrEmpty(usuario.Pass))
                {

                    // Verifica si la contraseña actual coincide con la almacenada
                    if (UsuarioDAL.HashMD5(currentPassword) != existingUser.Pass)
                    {
                        TempData["ErrorMessage"] = "La contraseña actual es incorrecta.";
                        return RedirectToAction("Perfil");
                    }

                    // Verificar que la nueva contraseña y la confirmación coincidan
                    if (usuario.Pass != usuario.ConfirmarPass)
                    {
                        TempData["ErrorMessage"] = "La nueva contraseña y la confirmación no coinciden.";
                        return RedirectToAction("Perfil");
                    }

                    existingUser.Pass = UsuarioDAL.HashMD5(usuario.Pass);
                }

                // Actualiza los campos comunes para todos los usuarios
                existingUser.Nombre = usuario.Nombre;
                existingUser.Apellido = usuario.Apellido;
                existingUser.Telefono = usuario.Telefono;
                existingUser.FechaNacimiento = usuario.FechaNacimiento;
                existingUser.NombreUsuario = usuario.NombreUsuario;

                // Permitir que el administrador cambie los campos adicionales solo si es su propio perfil
                if (User.IsInRole("Administrador") && existingUser.Id == usuario.Id)
                {
                    existingUser.Cargo = usuario.Cargo;
                    existingUser.Status = usuario.Status;
                }

                // Actualiza el usuario en la base de datos
                await _usuarioBL.Update(existingUser);
                TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
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
                return Json(new { success = true, message = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(new { success = false, message = $"Error al eliminar el usuario: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOwn()
        {

            // Verifica si el usuario está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Usuario no autenticado.";
                return RedirectToAction("Login", "Usuario");
            }

             var nombreUsuario = User.Identity.Name;
             Debug.WriteLine($"Valor de nombreUsuario: '{nombreUsuario}'");

            // Se verifica si userId es null o vacío
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                TempData["ErrorMessage"] = "No se pudo encontrar el usuario.";
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
                TempData["SuccessMessage"] = "Cuenta eliminada correctamente.";
                return RedirectToAction("Login", "Usuario");
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la cuenta.";
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
                var userDb = await _usuarioBL.LoginAsync(user);
                if (userDb != null && userDb.Id > 0 && userDb.NombreUsuario == user.NombreUsuario)
                {
                    userDb.Cargo = await cargoBL.GetById(new Cargo { Id = userDb.IdCargo });
                    var claims = new[] {
                     new Claim(ClaimTypes.Name, userDb.NombreUsuario),
                    new Claim(ClaimTypes.Role, userDb.Cargo.Nombre),
                    new Claim("Nombre", userDb.Nombre),
                    new Claim("Apellido", userDb.Apellido),
                    new Claim(ClaimTypes.NameIdentifier, userDb.Id.ToString())
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
    }

}