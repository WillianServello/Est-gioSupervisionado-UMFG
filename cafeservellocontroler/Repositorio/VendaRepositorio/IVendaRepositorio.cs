using cafeservellocontroler.Models.Venda;

namespace cafeservellocontroler.Repositorio.VendaRepositorio
{
    public interface IVendaRepositorio
    {
        Task<ModelVenda?> ListarPorId(int id);

        Task<List<ModelVenda>> BuscarTodasVendas();
        Task<List<ModelVenda>> BuscarVendasPorUsuario(int usuarioId);
        ModelVenda BuscarPorIdComItens(int id);
        ModelVenda Adicionar(ModelVenda venda);
        ModelVenda Atualizar(ModelVenda venda);
       
        bool Apagar (int id);
    }
}
