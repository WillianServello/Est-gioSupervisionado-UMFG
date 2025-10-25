using cafeservellocontroler.Data;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.Pessoa.ViewModels;
using cafeservellocontroler.Repositorio.RevendedorRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    public class RevendedorController : Controller
    {

        public readonly IRevendedorRepositorio _revendedorRepositorio;
        public RevendedorController(IRevendedorRepositorio revendedorRepositorio)
        {
            _revendedorRepositorio = revendedorRepositorio;
        }

        public IActionResult Index()
        {
            //buscar as informacoes no banco de dados
            var revendedores = _revendedorRepositorio.BuscarTodos();

            //agora ta na hora de mostrar esse objeto na tela, so que precisamos converter o ModelRevendedor para RevendedorViewModel
            var viewModels = revendedores.Select(r => new RevendedorViewModel
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

        public IActionResult Editar(int Id)
        {
            ModelRevendedor revendedor = _revendedorRepositorio.ListarPorId(Id);
            return PartialView(revendedor);
        }

        public IActionResult ApagarConfirmacao(int Id)
        {
            var revendedor = _revendedorRepositorio.ListarPorId(Id);
            if (revendedor == null) return NotFound();

            return PartialView("~/Views/Revendedor/ApagarConfirmacao.cshtml", revendedor);
        }

        [HttpPost]
        public IActionResult Criar(RevendedorViewModel model)
        {
            //Aqui ele esta fazendo a entrega de infomacoes, ele ta pegando as informacoes do RevendedorViewModel e passando para o ModelRevendedor pelo controller, o que esta dentro
            // do parenteses e por pq e necessario, o que esta fora e pq e opcional

            var revendedor = new ModelRevendedor(

                //necessario
                
                model.Nome,
                model.Telefone,
                model.Cnpj,
                model.Endereco,
                model.NomeFantasia
                );

            //opcional
            revendedor.Email = model.Email;
           
            //adicionando ao repositorio
            _revendedorRepositorio.Adicionar(revendedor);

            //direto pro Index
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Alterar(RevendedorViewModel model)
        {
            var revendedorExistente = _revendedorRepositorio.ListarPorId(model.Id);
            if (revendedorExistente == null)
            {
                return NotFound("Revendedor não encontrado.");
            }

            
            revendedorExistente.AtualizarDados(model);
            

            // Chama o repositório para salvar as alterações
            _revendedorRepositorio.Atualizar(revendedorExistente);
            return RedirectToAction("Index");
        }

        public IActionResult Apagar(int Id)
        {
            try
            {
                bool apagado = _revendedorRepositorio.Apagar(Id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Revendedor apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Erro ao apagar o revendedor.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao apagar o revendedor: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
