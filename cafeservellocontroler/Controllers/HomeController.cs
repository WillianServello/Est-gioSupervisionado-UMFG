using System.Diagnostics;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ModelPessoa pessoa = new ModelPessoa();

            pessoa.Nome = "Dirceu Servello";
            pessoa.Email = "dirceuservello@hotmail.com";
            return View(pessoa);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
