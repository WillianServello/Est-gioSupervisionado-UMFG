using cafeservellocontroler.Models.ViewModels;

namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelPessoa
    {
        

        public string Nome { get; private set; }
        public string Cnpj { get; private set; }
        public string Email { get; set; }
        public string Telefone { get; set; } 
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ModelPessoa(string nome, string cnpj, string telefone)
        {
            Nome = nome;
            Cnpj = cnpj;
            Telefone = telefone;
        }

        protected ModelPessoa()
        {
        }

        

        public void SetNome (string nome)
        {
            Nome = nome;
        }

        public void SetCnpj(string cnpj)
        {
            Cnpj = cnpj;
        }


    }
}
