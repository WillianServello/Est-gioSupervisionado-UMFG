using cafeservellocontroler.Data;
using cafeservellocontroler.Filters;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.ViewModels;
using cafeservellocontroler.Repositorio.RevendedorRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    [PaginaUsuarioLogado]
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
                Email = r.Email,
                DataCadastro = r.DataCadastro

            }).ToList();

            return View(viewModels);
        }

        public IActionResult Criar()
        {
            return PartialView();
        }

        public IActionResult Editar(int id)
        {
            ModelRevendedor revendedor = _revendedorRepositorio.ListarPorId(id);
            return PartialView(revendedor);
        }

        public IActionResult ApagarConfirmacao(int Id)
        {
            var revendedor = _revendedorRepositorio.ListarPorId(Id);
            if (revendedor == null) return NotFound();

            return PartialView("~/Views/Revendedor/ApagarConfirmacao.cshtml", revendedor);
        }

        public IActionResult Detalhes(int id)
        {
            ModelRevendedor revendedor = _revendedorRepositorio.ListarPorId(id);
            return PartialView("_Detalhes", revendedor);
        }

        [HttpPost]
        public IActionResult Criar(RevendedorViewModel model)
        {
            //Aqui ele esta fazendo a entrega de infomacoes, ele ta pegando as informacoes do RevendedorViewModel e passando para o ModelRevendedor pelo controller, o que esta dentro
            // do parenteses e por pq e necessario, o que esta fora e pq e opcional
            try { 
            var revendedor = new ModelRevendedor(

                //necessario

                model.Nome,
                model.Telefone,
                model.Cnpj,
                model.Endereco,
                model.NomeFantasia
                );

                revendedor.CriacaoDataCadastro();

                //opcional
                revendedor.Email = model.Email;

                //if (_usuarioRepositorio.LoginExistente(model.Login))
                //{
                //    TempData["MensagemErro"] = "Já existe um usuário com esse login.";
                //    return RedirectToAction("Index");
                //}


                if(_revendedorRepositorio.NomeExiste(model.Nome, model.Id))
                {
                    TempData["MensagemErro"] = "Já existe um revendedor com esse nome.";
                    return RedirectToAction("Index");
                }

                if(_revendedorRepositorio.NomeFantasiaExiste(model.NomeFantasia, model.Id))
                {
                    TempData["MensagemErro"] = "Já existe um revendedor com esse nome fantasia.";
                    return RedirectToAction("Index");
                }


                if(_revendedorRepositorio.CpfCnpjExiste(model.Cnpj, model.Id))
                {
                    TempData["MensagemErro"] = "Já existe um revendedor com esse CNPJ.";
                    return RedirectToAction("Index");
                }

                if (_revendedorRepositorio.EmailExiste(model.Email, model.Id))
                {
                    TempData["MensagemErro"] = "Já existe um revendedor com esse email.";
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
            {


                _revendedorRepositorio.Adicionar(revendedor);
                TempData["MensagemSucesso"] = "Revendedor cadastrado com sucesso";
                return RedirectToAction("Index");
            }
            return PartialView("_Criar", revendedor);
        }


        
            catch(System.Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possivel realizar o cadastro: {erro.Message}";
                return RedirectToAction("Index");

            
    } 
    
}


        [HttpPost]
        public IActionResult Alterar(RevendedorViewModel model)
        {
            var revendedorExistente = _revendedorRepositorio.ListarPorId(model.Id);

            if (revendedorExistente == null)
            {
                TempData["MensagemErro"] = "Revendedor não encontrado."; // Mensagem de erro se o produto não for encontrado
                return RedirectToAction("Index");
            }

            revendedorExistente.AtualizarDataCadastro();
            revendedorExistente.AtualizarDados(model);

            if (_revendedorRepositorio.NomeExiste(model.Nome, model.Id))
            {
                TempData["MensagemErro"] = "Já existe um revendedor com esse nome.";
                return RedirectToAction("Index");
            }

            if (_revendedorRepositorio.NomeFantasiaExiste(model.NomeFantasia, model.Id))
            {
                TempData["MensagemErro"] = "Já existe um revendedor com esse nome fantasia.";
                return RedirectToAction("Index");
            }


            if (_revendedorRepositorio.CpfCnpjExiste(model.Cnpj, model.Id))
            {
                TempData["MensagemErro"] = "Já existe um revendedor com esse CNPJ.";
                return RedirectToAction("Index");
            }

            if (_revendedorRepositorio.EmailExiste(model.Email, model.Id))
            {
                TempData["MensagemErro"] = "Já existe um revendedor com esse email.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _revendedorRepositorio.Atualizar(revendedorExistente);
                TempData["MensagemSucesso"] = "Revendedor atualizado com sucesso";
                return RedirectToAction("Index");
            }

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
                TempData["MensagemErro"] = $"Erro ao apagar o revendedor. Ele está em uma Venda!";
                return RedirectToAction("Index");
            }
        }
    }
}
