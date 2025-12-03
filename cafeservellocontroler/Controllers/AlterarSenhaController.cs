using cafeservellocontroler.Filters;
using cafeservellocontroler.Helper;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.ViewModels;
using cafeservellocontroler.Repositorio.UsuarioRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    [PaginaUsuarioLogado]
    public class AlterarSenhaController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        public AlterarSenhaController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Editar()
        {
            var usuario = _sessao.BuscarSessaoUsuario();

            return PartialView();
            
        }

        


        public IActionResult EditarDadosGet()
        {
            var usuario = _sessao.BuscarSessaoUsuario();

            var model = new EditarDadosViewModel
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Email = usuario.Email
            };

            return PartialView("EditarDadosGet", model);
        }




        [HttpPost]
        public IActionResult EditarDados(EditarDadosViewModel model)
        {
            ModelUsuario usuarioLogado = _sessao.BuscarSessaoUsuario();
            model.Id = usuarioLogado.Id;


            if (_usuarioRepositorio.LoginExistente(model.Login, model.Id))
            {
                TempData["MensagemErro"] = "Já existe um usuário com esse login.";
                return RedirectToAction("Index");
            }
            if (_usuarioRepositorio.EmailExistente(model.Email, model.Id))
            {
                TempData["MensagemErro"] = "Já existe um usuário com esse Email.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                // Atualiza no banco
                ModelUsuario usuarioAtualizado = _usuarioRepositorio.AlterarDados(model);

                // 🔥 Atualiza a sessão para refletir os novos dados
                _sessao.CriarSessaoUsuario(usuarioAtualizado);

                TempData["MensagemSucesso"] = "Dados atualizados com sucesso!";
                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Não foi possível atualizar os dados.";
            return RedirectToAction("Index");
        }









        [HttpPost]
        public IActionResult Alterar(ModelAlterarSenhaAtual modelAlterarSenhaAtual)
        {
            try
            {
                ModelUsuario usuarioLogado = _sessao.BuscarSessaoUsuario();
                modelAlterarSenhaAtual.Id = usuarioLogado.Id;
                if (ModelState.IsValid)
                {
                    
                    _usuarioRepositorio.AlterarSenha(modelAlterarSenhaAtual);

                    TempData["MensagemSucesso"] = "Senha atualizada com sucesso";
                    return RedirectToAction("Index", modelAlterarSenhaAtual);
                }
                return RedirectToAction("Index", modelAlterarSenhaAtual);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possivel alterar sua senha: {ex.Message} ";
                return RedirectToAction("Index", modelAlterarSenhaAtual);
            }
        }

        
    }


}
