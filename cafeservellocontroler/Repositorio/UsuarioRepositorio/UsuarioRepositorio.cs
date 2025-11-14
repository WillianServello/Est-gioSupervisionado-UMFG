using cafeservellocontroler.Data;
using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Repositorio.UsuarioRepositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        public readonly BancoContext _bancoContext;
        public UsuarioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ModelUsuario Adicionar(ModelUsuario usuario)
        {
            _bancoContext.Usuarios.Add(usuario);
            _bancoContext.SaveChanges();
            return usuario;
        }

        public bool Apagar(int id)
        {
            var usuario = _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);
            
            if (usuario == null)
                throw new Exception("Usuario não encontrado para exclusão.");

            _bancoContext.Usuarios.Remove(usuario);
            _bancoContext.SaveChanges();
            return true;
        }

        public ModelUsuario Atualizar(ModelUsuario usuario)
        {
            
            _bancoContext.Usuarios.Update(usuario);
            _bancoContext.SaveChanges();
            return usuario;
        }

        public List<ModelUsuario> BuscarTodos()
        {
            return _bancoContext.Usuarios.ToList();
        }

        public ModelUsuario ListarPorId(int id)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Id == id);
        }
    }
}
