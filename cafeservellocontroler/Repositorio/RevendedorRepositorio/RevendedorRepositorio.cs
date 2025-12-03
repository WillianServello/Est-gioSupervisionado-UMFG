using cafeservellocontroler.Data;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.RevendedorRepositorio
{
    public class RevendedorRepositorio : IRevendedorRepositorio
    {
        //Conexao com o banco de dados
        public readonly BancoContext _bancoContext;
        public RevendedorRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }


        public ModelRevendedor Adicionar(ModelRevendedor revendedor)
        {
            _bancoContext.Revendedor.Add(revendedor);
            _bancoContext.SaveChanges();
            return revendedor;
        }


        public ModelRevendedor Atualizar(ModelRevendedor revendedor)
        {

           
            _bancoContext.Revendedor.Update(revendedor);
            _bancoContext.SaveChanges();
            return revendedor;






        }

        public List<ModelRevendedor> BuscarTodos()
        {
            return _bancoContext.Revendedor.ToList();
        }

        public ModelRevendedor? ListarPorId(int id)
        {
            return _bancoContext.Revendedor.FirstOrDefault(x => x.Id == id);
        }

        public bool Apagar(int id)
        {
            var revendedor = _bancoContext.Revendedor.FirstOrDefault(x => x.Id == id);

            if (revendedor == null)
                throw new Exception("Revendedor não encontrado para exclusão.");

            _bancoContext.Revendedor.Remove(revendedor);
            _bancoContext.SaveChanges();

            return true;
        }










        public bool NomeExiste(string nome, int IdIgnorar)
        {
            return _bancoContext.Revendedor.Any(x => x.Nome.ToLower() == nome.ToLower() && x.Id != IdIgnorar);   
        }

        public bool NomeFantasiaExiste(string nomeFantasia, int IdIgnorar)
        {
            return _bancoContext.Revendedor.Any(x => x.NomeFantasia.ToLower() == nomeFantasia.ToLower() && x.Id != IdIgnorar);
        }

        public bool CpfCnpjExiste(string cpfCnpj , int IdIgnorar)
        {
            return _bancoContext.Revendedor.Any(x => x.Cnpj.ToLower() == cpfCnpj.ToLower() && x.Id != IdIgnorar);
        }

        public bool EmailExiste(string email , int IdIgnorar)
        {
            return _bancoContext.Revendedor.Any(x => x.Email.ToLower() == email.ToLower() && x.Id != IdIgnorar);
        }
    }
}
