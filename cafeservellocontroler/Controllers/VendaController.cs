using cafeservellocontroler.Enums;
using cafeservellocontroler.Filters;
using cafeservellocontroler.Helper;
using cafeservellocontroler.Models.Venda;
using cafeservellocontroler.Models.ViewModels;
using cafeservellocontroler.Repositorio.ProdutoRepositorio;
using cafeservellocontroler.Repositorio.RevendedorRepositorio;
using cafeservellocontroler.Repositorio.UsuarioRepositorio;
using cafeservellocontroler.Repositorio.VendaRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    [PaginaUsuarioLogado]
    public class VendaController : Controller
    {
        private readonly IRevendedorRepositorio _revendedorRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IVendaRepositorio _vendaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;

        public VendaController(IRevendedorRepositorio revendedorRepositorio,
                               IProdutoRepositorio produtoRepositorio,
                               IVendaRepositorio vendaRepositorio,
                               ISessao sessao,
                               IUsuarioRepositorio usuarioRepositorio)
        {
            _revendedorRepositorio = revendedorRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _vendaRepositorio = vendaRepositorio;
            _sessao = sessao;
            _usuarioRepositorio = usuarioRepositorio;
        }

        // LISTAGEM
        public async Task<IActionResult> Index()
        {
            var usuarioLogado = _sessao.BuscarSessaoUsuario();

            List<ModelVenda> vendas;

            // Se for administrador → lista tudo
            if (usuarioLogado.Perfil == PerfilEnum.Admin)
            {
                vendas = await _vendaRepositorio.BuscarTodasVendas();
            }
            else // Funcionário → somente vendas dele
            {
                vendas = await _vendaRepositorio.BuscarVendasPorUsuario(usuarioLogado.Id);
            }

            return View(vendas);
        }

        // VIEW DE CRIAÇÃO
        public IActionResult Criar()
        {
            var viewModel = new VendaViewModel
            {
                Revendedores = _revendedorRepositorio.BuscarTodos(),
                Produtos = _produtoRepositorio.BuscarTodos(),
                Itens = new List<ItemVendaViewModel> { new ItemVendaViewModel() }
            };

            return PartialView(viewModel);
        }


        public IActionResult ApagarConfirmacao(int Id) {

            var venda = _vendaRepositorio.BuscarPorIdComItens(Id);
            return PartialView(venda);
        }

        public IActionResult Editar(int id)
        {
            var venda = _vendaRepositorio.BuscarPorIdComItens(id);

            if (venda == null)
            {
                TempData["MensagemErro"] = "Venda não encontrada!";
                return RedirectToAction("Index");
            }

            var vm = new VendaViewModel
            {
                Id = venda.Id,
                RevendedorId = venda.Revendedor.Id,  // Agora vai existir!

                Itens = venda.ItensVendas.Select(iv => new ItemVendaViewModel
                {

                    EstoqueProduto = iv.Produto.Estoque + iv.Quantidade,
                    Id = iv.Id,
                    
                    ProdutoId = iv.Produto.Id,
                    Quantidade = iv.Quantidade,
                    Valor = iv.Valor,

                    PrecoProduto = iv.Produto.Preco
                   
                }).ToList(),

                Revendedores = _revendedorRepositorio.BuscarTodos(),
                Produtos = _produtoRepositorio.BuscarTodos()
            };

            return PartialView("Editar", vm);
        }

        public IActionResult Detalhes(int id)
        {
            var venda = _vendaRepositorio.BuscarPorIdComItens(id);
            if (venda == null)
            {
                TempData["MensagemErro"] = "Venda não encontrada!";
                return RedirectToAction("Index");
            }
            return PartialView("Detalhes", venda);
        }

        // SALVAR VENDA
        [HttpPost]
        public IActionResult Criar(VendaViewModel vm)
        {
            try
            {
                // 1. Usuário logado
                var usuario = _sessao.BuscarSessaoUsuario();

                if (usuario == null || usuario.Id <= 0)
                {
                    TempData["MensagemErro"] = "Sessão expirada. Faça login novamente.";
                    return RedirectToAction("Index", "Login");
                }

                // 2. Revendedor
                var revendedor = _revendedorRepositorio.ListarPorId(vm.RevendedorId);

                if (revendedor == null)
                {
                    TempData["MensagemErro"] = "Revendedor inválido.";
                    return RedirectToAction("Index");
                }

                // 3. Criar venda
                var venda = new ModelVenda(usuario, revendedor);

                // 4. Processar itens
                foreach (var itemVM in vm.Itens.Where(i => i.ProdutoId > 0 && i.Quantidade > 0))
                {
                    var produto = _produtoRepositorio.ListarPorId(itemVM.ProdutoId);

                    if (produto == null)
                        continue;

                    try
                    {
                        // ✔ Abate de estoque (usa seu método do Model)
                        produto.AbaterEstoque(itemVM.Quantidade);

                        // ✔ Atualiza produto no banco
                        _produtoRepositorio.Atualizar(produto);

                        // ✔ Cria ModelItensVenda
                        venda.AdicionarItem(produto, itemVM.Quantidade);
                    }
                    catch (InvalidOperationException ex)
                    {
                        TempData["MensagemErro"] = $"{produto.Nome}: {ex.Message}";
                        return RedirectToAction("Index");
                    }
                }

                // Nenhum item válido?
                if (!venda.ItensVendas.Any())
                {
                    TempData["MensagemErro"] = "A venda precisa ter pelo menos um item.";
                    return RedirectToAction("Index");
                }

                // 5. Gravar venda
                _vendaRepositorio.Adicionar(venda);

                TempData["MensagemSucesso"] =
                    $"Venda realizada com sucesso! Total: R$ {venda.TotalVenda:N2}.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao registrar venda: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Atualizar(VendaViewModel vm)
        {
            try
            {
                // 1. Usuário logado
                var usuarioSessao = _sessao.BuscarSessaoUsuario();
                var usuario = _usuarioRepositorio.ListarPorId(usuarioSessao.Id);

                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Sessão expirada.";
                    return RedirectToAction("Index", "Login");
                }

                // 2. Busca venda original com itens
                var vendaDB = _vendaRepositorio.BuscarPorIdComItens(vm.Id);
                if (vendaDB == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada.";
                    return RedirectToAction("Index");
                }

                // 3. BUSCAR REVENDEDOR NOVO
                var novoRevendedor = _revendedorRepositorio.ListarPorId(vm.RevendedorId);
                if (novoRevendedor == null)
                {
                    TempData["MensagemErro"] = "Revendedor inválido.";
                    return RedirectToAction("Index");
                }

                // 4. DEVOLVER ESTOQUE dos itens antigos
                foreach (var itemAntigo in vendaDB.ItensVendas)
                {
                    var produto = _produtoRepositorio.ListarPorId(itemAntigo.Produto.Id);
                    produto.AdicionarEstoque(itemAntigo.Quantidade);
                    _produtoRepositorio.Atualizar(produto);
                }

                // 5 LIMPAR itens antigos da venda
                vendaDB.ItensVendas.Clear();

                // 6. Atualizar protagonistas
                vendaDB.AtualizarVenda(usuario, novoRevendedor);

                // 7. Recriar os itens da venda usando o ViewModel
                foreach (var itemVM in vm.Itens.Where(i => i.ProdutoId > 0 && i.Quantidade > 0))
                {
                    var produto = _produtoRepositorio.ListarPorId(itemVM.ProdutoId);

                    // ABATER estoque do novo item
                    produto.AbaterEstoque(itemVM.Quantidade);
                    _produtoRepositorio.Atualizar(produto);

                    // Criar novo item de venda
                    vendaDB.AdicionarItem(produto, itemVM.Quantidade);
                }

                // 8. Salvar no banco
                vendaDB.AtualizarDataVenda();
                _vendaRepositorio.Atualizar(vendaDB);

                TempData["MensagemSucesso"] = "Venda atualizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao atualizar: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(int id)
        {
            try
            {
                var venda = _vendaRepositorio.BuscarPorIdComItens(id);

                if (venda == null)
                {
                    TempData["MensagemErro"] = "Venda não encontrada!";
                    return RedirectToAction("Index");
                }

                // Aqui ele devolve os itens ao estoque
                foreach (var item in venda.ItensVendas)
                {
                    var produto = _produtoRepositorio.ListarPorId(item.Produto.Id);

                    if (produto != null)
                    {
                        produto.Estoque += item.Quantidade;
                        _produtoRepositorio.Atualizar(produto);
                    }
                }

                
                _vendaRepositorio.Apagar(id);

                TempData["MensagemSucesso"] = "Venda removida e estoque atualizado!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao excluir a venda: {ex.Message}";
                return RedirectToAction("Index");
            }
        }



    }
}
