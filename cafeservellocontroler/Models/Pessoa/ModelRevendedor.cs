namespace cafeservellocontroler.Models.Pessoa
{
    public class ModelRevendedor : ModelPessoa
    {
        public int Id { get; private set;}
        public string Cnpj { get; private set;}
        public string Endereco { get; private set;}
        public string NomeFantasia { get; private set;}

    }
}
