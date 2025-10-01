using cafeservellocontroler.Models;
using cafeservellocontroler.Repositorio.ProdutoRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{

    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        public ProdutoController(IProdutoRepositorio produtoRepositorio) {
            _produtoRepositorio = produtoRepositorio;
        }
        public IActionResult Index()
        {
            List<ModelProduto> produtos = _produtoRepositorio.BuscarTodos();
            return View(produtos);
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
            ModelProduto produto = _produtoRepositorio.ListarPorId(id);
            return PartialView("_ApagarConfirmacao", produto);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _produtoRepositorio.Apagar(id);
                if(apagado)
                {
                    TempData["MensagemSucesso"] = "Contato apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Não foi possivel apagar seu contato!";
                }

                return RedirectToAction("Index");
            }
            catch(System.Exception erro)
            {

                TempData["MensagemErro"] = $"Não foi possivel apagar seu contato: {erro.Message}!";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Criar(ModelProduto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _produtoRepositorio.Adicionar(produto);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso";
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
        public IActionResult Alterar(ModelProduto produto)
        {


            try
            {
                if (ModelState.IsValid)
                {
                    _produtoRepositorio.Atualizar(produto);
                    TempData["MensagemSucesso"] = "Contato atualizado com sucesso";
                    return RedirectToAction("Index");
                }
                return PartialView("_Editar", produto);

            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possivel atualizar cadastro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
