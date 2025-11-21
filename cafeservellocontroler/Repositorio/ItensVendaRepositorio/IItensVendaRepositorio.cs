using cafeservellocontroler.Models.Venda;

namespace cafeservellocontroler.Repositorio.ItensVendaRepositorio
{
    public interface IItensVendaRepositorio
    {
        ModelItensVenda BuscarPorId(int id);

        List<ModelItensVenda> BuscarTodos();

        ModelItensVenda Adicionar(ModelItensVenda itensVenda);

    }
}
