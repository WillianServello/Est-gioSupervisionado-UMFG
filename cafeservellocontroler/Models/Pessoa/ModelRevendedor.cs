using cafeservellocontroler.Models.Pessoa.ViewModels;

namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelRevendedor : ModelPessoa
    {
        public int Id { get; private set;}
        public string Cnpj { get; private set;}
        public string Endereco { get; set;}
        public string NomeFantasia { get; private set;}


        public ModelRevendedor(string nome, string cnpj, string endereco, string nomeFantasia) : base(nome)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(endereco);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(nomeFantasia);

            Cnpj = cnpj;
            Endereco = endereco;
            NomeFantasia = nomeFantasia;
        }

        protected ModelRevendedor()
        {
        }

        public void SetCnpj(string cnpj)
        {
            Cnpj = cnpj;
        }

        public void SetNomeFantasia(string nomeFantasia)
        {
            NomeFantasia = nomeFantasia;

        }

        public void AtualizarDados(RevendedorViewModel viewModel)
        {
            SetNome(viewModel.Nome);
            Telefone = viewModel.Telefone;
            Email = viewModel.Email;
            SetCnpj(viewModel.Cnpj);
            Endereco = viewModel.Endereco;
            SetNomeFantasia(viewModel.NomeFantasia);

        }
    }
}
