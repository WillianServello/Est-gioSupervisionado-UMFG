using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.RevendedorRepositorio
{
    public interface IRevendedorRepositorio
    {
        ModelRevendedor ListarPorId(int id);
        ModelRevendedor Adicionar(ModelRevendedor revendedor);
        ModelRevendedor Atualizar(ModelRevendedor revendedor);
        List<ModelRevendedor> BuscarTodos();
    }

}
