using cafeservellocontroler.Filters;
using cafeservellocontroler.Helper;
using cafeservellocontroler.Models.Login;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Repositorio.UsuarioRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
    
    public class LoginController : Controller
    {


        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;
        public LoginController(IUsuarioRepositorio usuarioRepositorio,
                                ISessao sessao, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }   


        public IActionResult Index()
        {
            if(_sessao.BuscarSessaoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }
        
        public IActionResult RecuperarSenha()
        {
            return View();
        }

        public IActionResult Sair()
        {
            
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]

        public IActionResult Entrar(ModelLogin modelLogin)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    ModelUsuario usuario = _usuarioRepositorio.BuscarPorLogin(modelLogin.Login);
                    if (usuario != null)
                    {
                        if (usuario.SenhaCorreta(modelLogin.Senha))
                        {
                            _sessao.RemoverSessaoUsuario();
                            _sessao.CriarSessaoUsuario(usuario);
                            return RedirectToAction("Index", "Home");

                        }

                        

                    }

                    TempData["MensagemErro"] = "Login ou senha invalidos! Por favor tente novamente!";
        
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possivel realizar seu Login! Por favor ligue o servidor!";
                return RedirectToAction("Index");
            }


        }

        [HttpPost]
       public IActionResult EnviarLinkParaRedefinirSenha(ModelRecuperarSenha modelRecuperarSenha)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    ModelUsuario usuario = _usuarioRepositorio.BuscarPorEmailELogin(modelRecuperarSenha.Email, modelRecuperarSenha.Login);
                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova senha é: {novaSenha}";

                        bool emailEnviado = _email.Enviar(usuario.Email, "Sistemas de controle de cafes - ALTERAR NOVA SENHA", mensagem);

                        if (emailEnviado)
                        {
                            _usuarioRepositorio.Atualizar(usuario);
                            TempData["MensagemSucesso"] = "Enviamos para seu email cadastrado uma nova senha!.";

                        }
                        else
                        {
                            TempData["MensagemErro"] = "Erro ao enviar seu email! Tente novamente!";

                        }

                        return RedirectToAction("Index", "Login");

                    }

                    TempData["MensagemErro"] = "Não conseguimos redefinir sua senha! Por favor verifique os dados informados.";

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possivel redefinir sua senha!";
                return RedirectToAction("Index");
            }


        }

    }
}
