using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.RevendedorRepositorio
{
    public interface IRevendedorRepositorio
    {
        ModelRevendedor ListarPorId(int id);
        ModelRevendedor Adicionar(ModelRevendedor revendedor);
        ModelRevendedor Atualizar(ModelRevendedor revendedor);
        List<ModelRevendedor> BuscarTodos();


        bool NomeExiste(string nome, int IdIgnorar);
        bool NomeFantasiaExiste(string nomeFantasia, int IdIgnorar);
        bool CpfCnpjExiste(string cpfCnpj, int IdIgnorar);
        bool EmailExiste(string email, int IdIgnorar);

        bool Apagar(int id);
    }

}
