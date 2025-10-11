using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    public class RevendedorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Criar()
        {
            return PartialView();
        }

        public IActionResult Editar()
        {
            return PartialView();
        }

        public IActionResult ApagarConfirmacao()
        {
            return PartialView();
        }
    }
}
