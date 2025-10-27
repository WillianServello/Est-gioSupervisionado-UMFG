namespace cafeservellocontroler.Models.ViewModels
{
    public class RevendedorViewModel
    {

        //fazer validacoes de entrada, required, format, length, etc
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string NomeFantasia { get; set; }


        
    }
}
