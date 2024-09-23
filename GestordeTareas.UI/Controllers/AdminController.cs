using Microsoft.AspNetCore.Mvc;

namespace GestordeTareas.UI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View(); // Retorna la vista Dashboard.cshtml
        }
    }
}
