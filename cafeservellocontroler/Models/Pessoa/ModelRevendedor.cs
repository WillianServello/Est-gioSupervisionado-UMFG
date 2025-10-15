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
    }
}
