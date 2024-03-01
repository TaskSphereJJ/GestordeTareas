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
            _usuarioBL = new UsuarioBL(); // Inicializamos la capa de negocio
        }

        // GET: UsuarioController
        public async Task<ActionResult> Index()
        {
            List<Usuario> Lista = await _usuarioBL.GetAllAsync();
            return View(Lista);
        }

        // GET: UsuarioController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var usuario = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            return View(usuario);
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
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
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var usuario = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            return View(usuario);
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
                return View(usuario);
            }
        }

        // GET: UsuarioController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _usuarioBL.GetByIdAsync(new Usuario { Id = id });
            return View(usuario);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Usuario usuario)
        {
            try
            {
                await _usuarioBL.Delete(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(usuario);
            }
        }
    }

}
