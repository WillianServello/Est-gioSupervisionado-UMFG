using cafeservellocontroler.Models;

namespace cafeservellocontroler.Repositorio.ProdutoRepositorio
{
    public interface IProdutoRepositorio
    {
        ModelProduto ListarPorId(int id);
        List<ModelProduto> BuscarTodos();
        ModelProduto Adicionar(ModelProduto produto);
        ModelProduto Atualizar (ModelProduto produto);

        bool Apagar(int id);

        
    }
}
