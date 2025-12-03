using cafeservellocontroler.Filters;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.ViewModels;
using cafeservellocontroler.Repositorio.ProdutoRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    [PaginaUsuarioLogado]
    [PaginaRestritaSomenteAdmin]
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        public ProdutoController(IProdutoRepositorio produtoRepositorio) {
            _produtoRepositorio = produtoRepositorio;
        }
        public IActionResult Index()
        {
            var produtos = _produtoRepositorio.BuscarTodos(); 

            var viewModels = produtos.Select(p => new ProdutoViewModel
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                PrecoCompra = p.PrecoCompra,
                Preco = p.Preco,
                Estoque = p.Estoque,
                DataCadastro = p.DataCadastro,
                

            }).ToList();

            return View(viewModels);
        }

        public IActionResult Criar()
        { 
            return PartialView();
        }

        public IActionResult Editar(int id)
        {
            ModelProduto produto = _produtoRepositorio.ListarPorId(id);
            return PartialView(produto);


        }

        public IActionResult ApagarConfirmacao(int id)
        {
            var produto = _produtoRepositorio.ListarPorId(id);
            if (produto == null) return NotFound();
            return PartialView("_ApagarConfirmacao", produto);
        }

        public IActionResult Detalhes(int id)
        {
            var produto = _produtoRepositorio.ListarPorId(id);

            return PartialView("_Detalhes", produto);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _produtoRepositorio.Apagar(id);
                if(apagado)
                {
                    TempData["MensagemSucesso"] = "Produto apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Não foi possivel apagar seu Produto!";
                }

                return RedirectToAction("Index");
            }
            catch(System.Exception erro)
            {

                TempData["MensagemErro"] = $"Não foi possivel apagar seu Produto. Ele está em uma Venda!";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Criar(ProdutoViewModel model)
        {
            try
            {
                var produto = new ModelProduto(

                        model.Nome,
                        model.PrecoCompra ?? 0,
                        model.Preco ?? 0,
                        model.Estoque ?? 0

                );

                
                produto.Descricao = model.Descricao;
                produto.DataCadastroCriacao();

                if (ModelState.IsValid)
                {
                    

                    _produtoRepositorio.Adicionar(produto);
                    TempData["MensagemSucesso"] = "Produto cadastrado com sucesso";
                    return RedirectToAction("Index");
                }
                return PartialView("_Criar", produto);


            }
            catch(System.Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possivel realizar o cadastro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(ProdutoViewModel model)
        {
            try
            {

                var produtoExistente = _produtoRepositorio.ListarPorId(model.Id);
                if (produtoExistente == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado."; // Mensagem de erro se o produto não for encontrado
                    return RedirectToAction("Index");
                }

                produtoExistente.AtualizarDados(model);
                produtoExistente.AtualizarCadastro();

                if (ModelState.IsValid)
                {
                    _produtoRepositorio.Atualizar(produtoExistente);
                    TempData["MensagemSucesso"] = "Produto atualizado com sucesso";
                    return RedirectToAction("Index");
                }
                return PartialView("_Editar", produtoExistente);

            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possivel atualizar cadastro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
