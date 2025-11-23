using System.Collections.Generic;

namespace cafeservellocontroler.Repositorio
{
    public interface IRelatorioRepositorio
    {
        object ObterProdutoMaisVendido();
        object ObterRevendedorMaisAtivo();
        IEnumerable<object> ObterLucroPorProduto();
    }
}
