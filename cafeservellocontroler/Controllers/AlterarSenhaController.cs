 using cafeservellocontroler.Helper;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Repositorio.UsuarioRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
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
