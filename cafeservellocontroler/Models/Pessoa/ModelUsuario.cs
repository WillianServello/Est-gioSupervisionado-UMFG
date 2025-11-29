using cafeservellocontroler.Enums;
using cafeservellocontroler.Helper;
using cafeservellocontroler.Models.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelUsuario
    {


        public ModelUsuario()
        {
        }
        //o JsonProperty consegue fazer que a Session consiga intepretar esse set privado
        [JsonProperty]
        public int Id { get; private set; }
        public string Login { get; set; }
        public string Senha { get; private set; }
        public string Email { get; set; }
        public PerfilEnum Perfil { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime DataAtualizacaoCadastro { get; set; } = DateTime.Now;


        public ModelUsuario(string login, string senha, string email)
        {

            Login = login;
            Senha = senha;
            Email = email;
        }

        
        public void AtualizarDataCriacao()
        {
            DataAtualizacaoCadastro = DateTime.Now;
        }

        public void CriacaoDataCadastro()
        {
            DataCadastro = DateTime.Now;
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

        public ModelUsuario(int id, string login, string senha, string email, PerfilEnum perfil, DateTime dataCadastro, DateTime dataAtualizacaoCadastro)
        {
            // Atribuição direta para campos privados (só funciona DENTRO da classe)
            Id = id;
            Login = login;
            Senha = senha.GerarHash();
            Email = email;
            Perfil = perfil;
            DataCadastro = dataCadastro;
            DataAtualizacaoCadastro = dataAtualizacaoCadastro;
        }


    }
}
