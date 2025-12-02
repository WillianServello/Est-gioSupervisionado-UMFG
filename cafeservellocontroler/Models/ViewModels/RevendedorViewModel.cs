using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.ViewModels
{
    public class RevendedorViewModel
    {

        //fazer validacoes de entrada, required, format, length, etc
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Atingiu o limite de caracteres ")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo CNPJ é obrigatório")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Telefone é obrigatório")]
        public string Telefone { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Formato de e-mail inválido")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Endereço é obrigatório")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Nome Fantasia é obrigatório")]
        public string NomeFantasia { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime DataAtualizacaoCadastro { get; set; } = DateTime.Now;

    }
}
