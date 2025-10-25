using cafeservellocontroler.Models.Pessoa;
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
                Email = r.Email,
                Telefone = r.Telefone,
                MateriaPrima = r.MateriaPrima
            }).ToList();

            return View(viewModels);
        }

        public IActionResult Criar()
        {
            return PartialView();
        }

        public IActionResult Editar(int Id)
        {
            ModelFornecedor fornecedor = _fornecedorRepositorio.ListarPorId(Id);
            return PartialView(fornecedor);
        }

        public IActionResult ApagarConfirmacao(int Id)
        {
            var fornecedor = _fornecedorRepositorio.ListarPorId(Id);
            if (fornecedor == null) return NotFound();

            return PartialView("~/Views/Revendedor/ApagarConfirmacao.cshtml", fornecedor);
        }

        [HttpPost]
        public IActionResult Criar(FornecedorViewModel fornecedorView)
        {

            return PartialView();
        }
    }
}
