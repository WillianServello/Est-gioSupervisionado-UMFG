using cafeservellocontroler.Data;

namespace cafeservellocontroler.Repositorio
{
    public class RelatorioRepositorio : IRelatorioRepositorio
    {
        private readonly BancoContext _context;

        public RelatorioRepositorio(BancoContext context)
        {
            _context = context;
        }

        public object ObterProdutoMaisVendido()
        {
            return _context.ItensVendas
                .GroupBy(i => new { i.Produto.Id, i.Produto.Nome })
                .Select(g => new
                {
                    ProdutoId = g.Key.Id,
                    Nome = g.Key.Nome,
                    QuantidadeVendida = g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .FirstOrDefault();
        }

        public object ObterRevendedorMaisAtivo()
        {
            return _context.Vendas
                .GroupBy(v => new { v.Revendedor.Id, v.Revendedor.NomeFantasia })
                .Select(g => new
                {
                    RevendedorId = g.Key.Id,
                    Nome = g.Key.NomeFantasia,
                    TotalVendas = g.Count()
                })
                .OrderByDescending(x => x.TotalVendas)
                .FirstOrDefault();
        }

        public IEnumerable<object> ObterLucroPorProduto()
        {
            return _context.ItensVendas
                .GroupBy(i => new
                {
                    i.Produto.Id,
                    i.Produto.Nome,
                    PrecoCompra = i.Produto.PrecoCompra,
                    PrecoVenda = i.Produto.Preco
                })
                .Select(g => new
                {
                    ProdutoId = g.Key.Id,
                    Nome = g.Key.Nome,
                    PrecoCompra = g.Key.PrecoCompra,
                    PrecoVenda = g.Key.PrecoVenda,
                    LucroUnidade = g.Key.PrecoVenda - g.Key.PrecoCompra,
                    QuantidadeVendida = g.Sum(x => x.Quantidade),
                    LucroTotal = (g.Key.PrecoVenda - g.Key.PrecoCompra) * g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.LucroTotal)
                .ToList();
        }
    }
}
