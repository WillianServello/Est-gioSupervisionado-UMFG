using cafeservellocontroler.Models.Pessoa.ViewModels;
using cafeservellocontroler.Repositorio.FornecedorRepositorio;
using cafeservellocontroler.Repositorio.RevendedorRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    public class FornecedorController : Controller
    {

        public readonly IFornecedorRepositorio _fornecedorRepositorio;
        public FornecedorController(IFornecedorRepositorio fornecedorRepositorio)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }

        public IActionResult Index()
        {
            //buscar as informacoes no banco de dados
            var fornecedores = _fornecedorRepositorio.BuscarTodos();

            //agora ta na hora de mostrar esse objeto na tela, so que precisamos converter o ModelFornecedor para FornecedorViewModel
            var viewModels = fornecedores.Select(r => new FornecedorViewModel
            {
                Id = r.Id,
                Nome = r.Nome,
                Cnpj = r.Cnpj,
                Endereco = r.Endereco,
                NomeFantasia = r.NomeFantasia,
                Telefone = r.Telefone,
                Email = r.Email
            }).ToList();

            return View(viewModels);
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
