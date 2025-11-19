using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.UsuarioRepositorio
{
    public interface IUsuarioRepositorio
    {

        ModelUsuario BuscarPorLogin(string login);
        ModelUsuario BuscarPorEmailELogin(string login, string email);
        
        ModelUsuario ListarPorId(int id);
        ModelUsuario Adicionar(ModelUsuario usuario);
        ModelUsuario Atualizar(ModelUsuario usuario);
        List<ModelUsuario> BuscarTodos();

        ModelUsuario AlterarSenha(ModelAlterarSenhaAtual modelAlterarSenhaAtual);

        bool Apagar(int id);

    }
}
