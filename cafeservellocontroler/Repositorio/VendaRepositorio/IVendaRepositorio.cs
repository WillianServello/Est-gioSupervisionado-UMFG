using cafeservellocontroler.Models.Venda;

namespace cafeservellocontroler.Repositorio.VendaRepositorio
{
    public interface IVendaRepositorio
    {
        Task<ModelVenda?> ListarPorId(int id);

        Task<List<ModelVenda>> BuscarTodasVendas();
        ModelVenda BuscarPorIdComItens(int id);
        ModelVenda Adicionar(ModelVenda venda);
        ModelVenda Atualizar(ModelVenda venda);
        bool Apagar (int id);
    }
}
