using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.RevendedorRepositorio
{
    public interface IRevendedorRepositorio
    {
        ModelRevendedor Adicionar(ModelRevendedor revendedor);
        List<ModelRevendedor> BuscarTodos();
    }

}
