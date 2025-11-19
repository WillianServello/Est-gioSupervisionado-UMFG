using cafeservellocontroler.Enums;
using cafeservellocontroler.Helper;
using cafeservellocontroler.Models.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelUsuario
    {


        protected ModelUsuario()
        {
        }
        //o JsonProperty consegue fazer que a Session consiga intepretar esse set privado
        [JsonProperty]
        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Senha { get; private set; }
        public string Email { get; set; }
        public PerfilEnum Perfil { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;


        public ModelUsuario(string login, string senha, string email)
        {

            Login = login;
            Senha = senha;
            Email = email;
        }

        

        public void SetPerfil(PerfilEnum perfil)
        {
            Perfil = perfil;
        }

        public void AtualizarDados(UsuarioViewModel model)
        {
            Id = model.Id;
            Login = model.Login;
            Email = model.Email;
            Perfil = model.Perfil;

        }

      

        public bool SenhaCorreta(string senha)
        {
            return Senha == senha.GerarHash();
        }

        public void SetSenhaHash()
        {
            Senha = Senha.GerarHash();
        }

        public void SetNovaSenha(string novaSenha)
        {
            Senha = novaSenha.GerarHash();
        }

        public string GerarNovaSenha()
        { 
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.GerarHash();
            return novaSenha;
        }

       
    }
}
