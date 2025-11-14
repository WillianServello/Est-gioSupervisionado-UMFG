using cafeservellocontroler.Models.ViewModels;

namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelUsuario
    {
        

        protected ModelUsuario()
        {
        }

        public int Id { get; private set; }
        public string Login { get; private set; } 
        public string Senha { get; private set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ModelUsuario( string login, string senha, string email)
        {
           
            Login = login;
            Senha = senha;
            Email = email;
        }


        public void AtualizarDados(UsuarioViewModel model)
        {
            Id = model.Id;
            Login = model.Login;
            Senha = model.Senha;
            Email = model.Email;
            DataCadastro = model.DataCadastro;
        }

    }
}
