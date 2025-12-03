using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.ViewModels
{
    public class EditarDadosViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Login é obrigatório")]
        [MaxLength(100, ErrorMessage = "Atingiu o limite de caracteres ")]
        public string Login { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Formato de e-mail inválido")]
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }
    }
}
