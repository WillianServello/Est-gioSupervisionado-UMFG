using cafeservellocontroler.Filters;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.ViewModels;
using cafeservellocontroler.Repositorio.ProdutoRepositorio;
using cafeservellocontroler.Repositorio.UsuarioRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    [PaginaRestritaSomenteAdmin]
    [PaginaUsuarioLogado]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }


        public IActionResult Index()
        {

            var usuarios = _usuarioRepositorio.BuscarTodos();

            var viewModels = usuarios.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Login = u.Login,
                Senha = u.Senha,
                Email = u.Email,
                Perfil = u.Perfil,
                DataCadastro = u.DataCadastro

            }).ToList();


            return View(viewModels);
        }
        public IActionResult Criar()
        {
            return PartialView();
        }
        public IActionResult Editar(int id)
        {

            ModelUsuario usuario = _usuarioRepositorio.ListarPorId(id);
            return View();
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            var usuario = _usuarioRepositorio.ListarPorId(id);
            return PartialView(usuario);
        }
        public IActionResult Detalhes(int id)
        {
            var usuario = _usuarioRepositorio.ListarPorId(id);
            return PartialView("_Detalhes", usuario);
        }
        [HttpPost]
        public IActionResult Criar(UsuarioViewModel model)
            {

            try{ 
            var usuario = new ModelUsuario
                (
               
                model.Login,
                model.Senha,
                model.Email
                );

            usuario.SetPerfil(model.Perfil);
            usuario.CriacaoDataCadastro();



                if (ModelState.IsValid)
            {


                _usuarioRepositorio.Adicionar(usuario);
                TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso";
                return RedirectToAction("Index");
            }
            return PartialView("_Criar", usuario);
        }


        
            catch(System.Exception erro)
            {
                TempData["MensagemErro"] = $"Não foi possivel realizar o usuário: {erro.Message}";
                return RedirectToAction("Index");


    }
}

        [HttpPost]
        public IActionResult Alterar(UsuarioViewModel model)
        {


            var usuario = _usuarioRepositorio.ListarPorId(model.Id);
            if(usuario == null)
            {
                TempData["MensagemErro"] = "Não foi possivel encontrar usuário: ";
                return RedirectToAction("Index");
            }

                usuario.AtualizarDataCriacao();
            usuario.AtualizarDados(model);
                _usuarioRepositorio.Atualizar(usuario);
                TempData["MensagemSucesso"] = "Alteração realizado com sucesso! ";
          

            return RedirectToAction("Index");
        }


        public IActionResult Apagar(int Id)
        {
            try
            {
                bool apagado = _usuarioRepositorio.Apagar(Id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Usuário apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Erro ao apagar o usuário.";
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
