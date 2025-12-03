using cafeservellocontroler.Data;
using cafeservellocontroler.Models.Venda;
using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;

namespace cafeservellocontroler.Repositorio.VendaRepositorio
{
    public class VendaRepositorio : IVendaRepositorio
    {

        private readonly BancoContext _bancoDeDadosContext;
        public VendaRepositorio(BancoContext bancoDeDadosContext)
        {
            _bancoDeDadosContext = bancoDeDadosContext;
        }


        public ModelVenda Adicionar(ModelVenda venda)
        {
            // 🛑 CORREÇÃO 1: Tratar Usuario e Revendedor 🛑
            // Marca o usuário como existente (Unchanged) para que o EF Core 
            // use apenas o Id_Usuario.
            if (venda.Usuario != null)
            {
                _bancoDeDadosContext.Entry(venda.Usuario).State = EntityState.Unchanged;
            }

            // Marca o revendedor como existente (Unchanged) para que o EF Core 
            // use apenas o Id_Revendedor.
            if (venda.Revendedor != null)
            {
                _bancoDeDadosContext.Entry(venda.Revendedor).State = EntityState.Unchanged;
            }

            // 🛑 CORREÇÃO 2: Tratar os Produtos dentro dos ItensVendas 🛑
            // É crucial fazer o mesmo para os ModelProduto dentro de cada item, 
            // senão a FK falhará na tabela ItensVenda.
            foreach (var item in venda.ItensVendas)
            {
                if (item.Produto != null)
                {
                    _bancoDeDadosContext.Entry(item.Produto).State = EntityState.Unchanged;
                }
            }

            // Agora, o EF Core insere apenas a Venda e os ItensVendas, 
            // usando os IDs das entidades relacionadas.
            _bancoDeDadosContext.Vendas.Add(venda);
            _bancoDeDadosContext.SaveChanges();
            return venda;
        }


        public bool Apagar(int id)
        {
            var venda = _bancoDeDadosContext.Vendas.FirstOrDefault(x => x.Id == id);

            if (venda == null)
                return false;

            _bancoDeDadosContext.Vendas.Remove(venda);
            _bancoDeDadosContext.SaveChanges();
            return true;
        }


        public async Task<List<ModelVenda>> BuscarTodasVendas()
        {
            return await _bancoDeDadosContext.Vendas
                 .Include(x => x.Usuario)
                 .Include(x => x.Revendedor)
                 .Include(x => x.ItensVendas)
                 .ToListAsync();


        }
        public async Task<List<ModelVenda>> BuscarVendasPorUsuario(int usuarioId)
        {
            return await _bancoDeDadosContext.Vendas
                .Include(x => x.Usuario)
                .Include(x => x.Revendedor)
                .Include(x => x.ItensVendas)
                .Where(x => x.Usuario.Id == usuarioId)
                .ToListAsync();
        }

        public ModelVenda? BuscarPorIdComItens(int id)
        {
            return _bancoDeDadosContext.Vendas
                .Include(v => v.Usuario)
                .Include(v => v.Revendedor)
                .Include(v => v.ItensVendas)
                    .ThenInclude(iv => iv.Produto)
                .FirstOrDefault(v => v.Id == id);
        }

        public async Task<ModelVenda?> ListarPorId(int id)
        {
            return await _bancoDeDadosContext.Vendas
                 .Include(x => x.Usuario)
                 .Include(x => x.Revendedor)
                 .Include(x => x.ItensVendas)
                 .ThenInclude(iv => iv.Produto)
                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public ModelVenda Atualizar(ModelVenda venda)
        {
            if (venda.Usuario != null)
            {
                _bancoDeDadosContext.Entry(venda.Usuario).State = EntityState.Unchanged;
            }


            if (venda.Revendedor != null)
            {
                _bancoDeDadosContext.Entry(venda.Revendedor).State = EntityState.Unchanged;
            }


            foreach (var item in venda.ItensVendas)
            {
                if (item.Produto != null)
                {
                    _bancoDeDadosContext.Entry(item.Produto).State = EntityState.Unchanged;
                }
            }


            _bancoDeDadosContext.Vendas.Update(venda);
            _bancoDeDadosContext.SaveChanges();
            return venda;
        }

    }
}
