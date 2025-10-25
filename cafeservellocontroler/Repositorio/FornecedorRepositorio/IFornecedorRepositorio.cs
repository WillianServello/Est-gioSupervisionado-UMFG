using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.FornecedorRepositorio
{
    public interface IFornecedorRepositorio
    {
        ModelFornecedor ListarPorId(int id);
        List<ModelFornecedor> BuscarTodos();
        ModelFornecedor Adicionar(ModelFornecedor fornecedor);
        ModelFornecedor Atualizar(ModelFornecedor fornecedor);

        bool Apagar(int id);


    }
}
