using cafeservellocontroler.Models.Pessoa.ViewModels;

namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelFornecedor : ModelPessoa
    {
        public int Id { get; private set; }
        public string MateriaPrima { get; private set; }


        public ModelFornecedor(string nome, string cnpj, string telefone, string materiaprima) : base(nome, cnpj, telefone)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(MateriaPrima);
        }

        protected ModelFornecedor()
        {
        }

        public void SetMateriaPrima(string materiaPrima)
        {
            MateriaPrima = materiaPrima;
        }

        public void AtualizarDados(FornecedorViewModel viewModel)
        {
            SetNome(viewModel.Nome);
            Telefone = viewModel.Telefone;
            Email = viewModel.Email;
            SetCnpj(viewModel.Cnpj);
            MateriaPrima = viewModel.MateriaPrima;

        }
    }
}
