using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.ViewModels;
using cafeservellocontroler.Repositorio.ProdutoRepositorio;
using cafeservellocontroler.Repositorio.UsuarioRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace cafeservellocontroler.Controllers
{
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

        [HttpPost]
        public IActionResult Criar(UsuarioViewModel model)
            {

            var usuario = new ModelUsuario
                (
               
                model.Login,
                model.Senha,
                model.Email
                
                );

            usuario.DataCadastro = model.DataCadastro;

        

            _usuarioRepositorio.Adicionar(usuario);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Alterar(UsuarioViewModel model)
        {
            var usuario = _usuarioRepositorio.ListarPorId(model.Id);

            usuario.AtualizarDados(model);

            _usuarioRepositorio.Atualizar(usuario);

            return RedirectToAction("Index");
        }


        public IActionResult Apagar(int Id)
        {
            try
            {
                bool apagado = _usuarioRepositorio.Apagar(Id);

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
