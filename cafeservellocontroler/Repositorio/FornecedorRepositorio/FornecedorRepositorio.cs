using cafeservellocontroler.Data;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.FornecedorRepositorio
{
    public class FornecedorRepositorio : IFornecedorRepositorio
    {
        //Conexao com o banco de dados
        public readonly BancoContext _bancoContext;
        public FornecedorRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }


        public ModelFornecedor Adicionar(ModelFornecedor fornecedor)
        {
            _bancoContext.Fornecedor.Add(fornecedor);
            _bancoContext.SaveChanges();
            return fornecedor;
        }


        public <ModelFornecedor> Atualizar(ModelFornecedor fornecedor)
        {


            _bancoContext.Fornecedor.Update(fornecedor);
            _bancoContext.SaveChanges();
            return fornecedor;






        }

        public List<ModelFornecedor> BuscarTodos()
        {
            return _bancoContext.Fornecedor.ToList();
        }

        public ModelFornecedor ListarPorId(int id)
        {
            return _bancoContext.Fornecedor.FirstOrDefault(x => x.Id == id);
        }

        public bool Apagar(int id)
        {
            var fornecedor = _bancoContext.Fornecedor.FirstOrDefault(x => x.Id == id);

            if (fornecedor == null)
                throw new Exception("Fornecedor não encontrado para exclusão.");

            _bancoContext.Fornecedor.Remove(fornecedor);
            _bancoContext.SaveChanges();

            return true;
        }
    }
}
