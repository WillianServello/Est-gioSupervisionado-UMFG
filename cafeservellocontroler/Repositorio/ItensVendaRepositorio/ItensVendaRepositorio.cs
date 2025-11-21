using cafeservellocontroler.Data;
using cafeservellocontroler.Models.Venda;

namespace cafeservellocontroler.Repositorio.ItensVendaRepositorio
{
    public class ItensVendaRepositorio : IItensVendaRepositorio
    {
        private readonly BancoContext _bancoContext;

        public ItensVendaRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ModelItensVenda Adicionar(ModelItensVenda itensVenda)
        {
            _bancoContext.ItensVendas.Add(itensVenda);
            _bancoContext.SaveChanges();
            return itensVenda;
        }

        public ModelItensVenda? BuscarPorId(int id)
        {
            return _bancoContext.ItensVendas.FirstOrDefault(x => x.Id == id);
        }

        public List<ModelItensVenda> BuscarTodos()
        {
            return _bancoContext.ItensVendas.ToList();
        }
    }
}
