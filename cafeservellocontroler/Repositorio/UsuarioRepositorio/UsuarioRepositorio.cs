using cafeservellocontroler.Data;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.ViewModels;

namespace cafeservellocontroler.Repositorio.UsuarioRepositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        public readonly BancoContext _bancoContext;
        public UsuarioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        

        public ModelUsuario? BuscarPorLogin(string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Login.ToUpper() == login.ToUpper());

           
        }
        public ModelUsuario? BuscarPorEmailELogin(string email, string login)
        {
            return _bancoContext.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper() && x.Login.ToUpper() == login.ToUpper());
        }

        public ModelUsuario Adicionar(ModelUsuario usuario)
        {
            usuario.SetSenhaHash();
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

        public ModelUsuario AlterarSenha(ModelAlterarSenhaAtual modelAlterarSenhaAtual)
        {
            ModelUsuario modelUsuario = ListarPorId(modelAlterarSenhaAtual.Id);

            if (modelUsuario == null)
                throw new Exception("Usuário não encontrado.");

            if(!modelUsuario.SenhaCorreta(modelAlterarSenhaAtual.SenhaAtual))
                throw new Exception("Senha atual incorreta.");
            

            if(modelUsuario.SenhaCorreta(modelAlterarSenhaAtual.NovaSenha) )
                throw new Exception("A nova senha não pode ser igual a senha atual.");


            modelUsuario.SetNovaSenha(modelAlterarSenhaAtual.NovaSenha);

            _bancoContext.Usuarios.Update(modelUsuario);
            _bancoContext.SaveChanges();
            //fazer o datetime atualizacao aqui
            return modelUsuario;
        }

        public ModelUsuario AlterarDados(EditarDadosViewModel editarDadosViewModel)
        {
            ModelUsuario modelUsuario = ListarPorId(editarDadosViewModel.Id);

            if (modelUsuario == null)
                throw new Exception("Usuário não encontrado.");

            
            modelUsuario.AtualizarDadosDoPerfil(editarDadosViewModel);
            _bancoContext.Usuarios.Update(modelUsuario);
            _bancoContext.SaveChanges();

            return modelUsuario;
        }

        public bool LoginExistente(string login, int idIgnorar)
        {
            return _bancoContext.Usuarios.Any(x => x.Login.ToUpper() == login.ToUpper() && x.Id != idIgnorar);
        }

        public bool EmailExistente(string email, int idIgnorar)
        {
            return _bancoContext.Usuarios.Any(x => x.Email.ToUpper() == email.ToUpper() && x.Id != idIgnorar);
        }
    }
}
