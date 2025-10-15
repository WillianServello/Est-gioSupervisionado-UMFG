using cafeservellocontroler.Models.Pessoa.ViewModels;

namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelPessoa
    {
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public ModelPessoa(string nome)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(nome);
            Nome = nome;
        }

        protected ModelPessoa()
        {
        }

        public void SetNome (string nome)
        {
            Nome = nome;
        }

    
    }
}
