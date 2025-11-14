using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.UsuarioRepositorio
{
    public interface IUsuarioRepositorio
    {
        ModelUsuario ListarPorId(int id);
        ModelUsuario Adicionar(ModelUsuario usuario);
        ModelUsuario Atualizar(ModelUsuario usuario);
        List<ModelUsuario> BuscarTodos();

        bool Apagar(int id);

    }
}
