using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.ViewModels;
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
                Telefone = r.Telefone,
                Email = r.Email,
                MateriaPrima = r.MateriaPrima,
                DataCadastro = r.DataCadastro.ToUniversalTime()
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

            return PartialView("~/Views/Fornecedor/ApagarConfirmacao.cshtml", fornecedor);
        }

        [HttpPost]
        public IActionResult Criar(FornecedorViewModel model)
        {
            //Aqui ele esta fazendo a entrega de infomacoes, ele ta pegando as informacoes do FornecedorViewModel e passando para o ModelFornecedor pelo controller, o que esta dentro
            // do parenteses e por pq e necessario, o que esta fora e pq e opcional

            var fornecedor = new ModelFornecedor(

                //necessario

                model.Nome,
                model.Cnpj,
                model.Telefone,
                model.MateriaPrima
                );

            //opcional

            fornecedor.Email = model.Email;
            fornecedor.DataCadastro = model.DataCadastro;

            //adicionando ao repositorio
            _fornecedorRepositorio.Adicionar(fornecedor);

            //direto pro Index
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Alterar(FornecedorViewModel model)
        {
            var fornecedorExistente = _fornecedorRepositorio.ListarPorId(model.Id);
            if (fornecedorExistente == null)
            {
                return NotFound("Fornecedor não encontrado.");
            }


            fornecedorExistente.AtualizarDados(model);


            // Chama o repositório para salvar as alterações
            _fornecedorRepositorio.Atualizar(fornecedorExistente);
            return RedirectToAction("Index");
        }

        public IActionResult Apagar(int Id)
        {
            try
            {
                bool apagado = _fornecedorRepositorio.Apagar(Id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Fornecedor apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Erro ao apagar o fornecedor.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao apagar o fornecedor: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
