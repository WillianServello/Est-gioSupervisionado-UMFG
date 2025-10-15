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

        public IActionResult Editar()
        {
            return PartialView();
        }

        public IActionResult ApagarConfirmacao()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult Criar(RevendedorViewModel model)
        {
            //Aqui ele esta fazendo a entrega de infomacoes, ele ta pegando as informacoes do RevendedorViewModel e passando para o ModelRevendedor pelo controller, o que esta dentro
            // do parenteses e por pq e necessario, o que esta fora e pq e opcional

            var revendedor = new ModelRevendedor(

                //necessario
                
                model.Nome,
                model.Cnpj,
                model.Endereco,
                model.NomeFantasia
                );

            //opcional
            revendedor.Telefone = model.Telefone;
            revendedor.Email = model.Email;
           
            //adicionando ao repositorio
            _revendedorRepositorio.Adicionar(revendedor);

            //direto pro Index
            return RedirectToAction("Index");
        }
    }
}
