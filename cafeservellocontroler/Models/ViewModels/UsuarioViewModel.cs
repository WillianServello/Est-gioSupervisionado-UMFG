using cafeservellocontroler.Enums;
using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Login é obrigatório")]
        [MaxLength(100, ErrorMessage = "Atingiu o limite de caracteres ")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
        ErrorMessage = "A senha deve ter pelo menos uma letra, um número e 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Perfil é obrigatório")]
        public PerfilEnum Perfil { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime DataAtualizacaoCadastro { get; set; } = DateTime.Now;


    }
}
