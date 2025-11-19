using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Helper
{
    public interface ISessao
    {

        void CriarSessaoUsuario(ModelUsuario usuario);
        ModelUsuario BuscarSessaoUsuario();
        void RemoverSessaoUsuario();
    }
}
