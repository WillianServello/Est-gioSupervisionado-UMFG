using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Login é obrigatório")]
        [MaxLength(100, ErrorMessage = "Atingiu o limite de caracteres ")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo senha é obrigatório")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        public string Email { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
