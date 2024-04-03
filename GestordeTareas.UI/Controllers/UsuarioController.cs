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

namespace GestordeTareas.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        UsuarioBL _usuarioBL = new UsuarioBL();

        CargoBL cargoBL = new CargoBL();


        // GET: UsuarioController
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
            ViewBag.Top = user.Top_Aux;
            ViewBag.Roles = await cargoBL.GetAllAsync();
            return View(Lista);
        }


        // GET: UsuarioController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var user = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            user.Cargo = await cargoBL.GetById(new Cargo { Id = user.IdCargo });
            return View(user);
        }

        // GET: UsuarioController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Cargos = await cargoBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            try
            {
                int result = await _usuarioBL.Create(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Cargos = await cargoBL.GetAllAsync();
                return View(usuario);
            }
        }

        // GET: UsuarioController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var userDb = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            ViewBag.Cargos = await cargoBL.GetAllAsync();
            ViewBag.Error = "";
            return View(userDb);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Usuario usuario)
        {
            try
            {
                int result = await _usuarioBL.Update(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Cargos = await cargoBL.GetAllAsync();
                return View(usuario);
            }
        }

        // GET: UsuarioController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            user.Cargo = await cargoBL.GetById(new Cargo { Id = user.IdCargo });
            ViewBag.Error = "";
            return View(user);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Usuario usuario)
        {
            try
            {
                int result = await _usuarioBL.Delete(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var userDb = await _usuarioBL.GetByIdAsync(usuario);
                if (userDb == null)
                    userDb = new Usuario();
                if (userDb.Id > 0)
                    userDb.Cargo = await cargoBL.GetById(new Cargo { Id = userDb.IdCargo });
                return View(userDb);
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
                    var claims = new[] {new Claim(ClaimTypes.Name, userDb.NombreUsuario),
                                new Claim(ClaimTypes.Role, userDb.Cargo.Nombre)};
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                }
                else
                    throw new Exception("Credenciales de usuario incorrectas");

                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
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