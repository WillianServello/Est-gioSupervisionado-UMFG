using cafeservellocontroler.Data;
using cafeservellocontroler.Models;
using Npgsql;
using System.Data.Common;

namespace cafeservellocontroler.Repositorio.ProdutoRepositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {   
        private readonly BancoContext _bancoContext;
        public ProdutoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;

        }
        public ModelProduto ListarPorId(int id)
        {
            return _bancoContext.Produtos.FirstOrDefault(x => x.Id == id);
        }
        public List<ModelProduto> BuscarTodos()
        {
            return _bancoContext.Produtos.ToList();
        }
        public ModelProduto Adicionar(ModelProduto produto)
        {
            _bancoContext.Produtos.Add(produto);
            _bancoContext.SaveChanges();
            return produto;
        }

        public ModelProduto Atualizar(ModelProduto produto)
        {

            if (produto == null) throw new Exception("Houve um erro na atualização do produto"); 

            _bancoContext.Produtos.Update(produto);
            _bancoContext.SaveChanges();

            return produto;
         
        }

        public bool Apagar(int id)
        {
            ModelProduto produtoDB = ListarPorId(id);

            if (produtoDB == null) throw new Exception("Houve um erro ao deleter o produto");

            _bancoContext.Produtos.Remove(produtoDB);
            _bancoContext.SaveChanges();

            return true;
        }
    }
}
