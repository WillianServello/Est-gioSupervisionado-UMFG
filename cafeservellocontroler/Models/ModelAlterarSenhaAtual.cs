using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models
{
    public class ModelAlterarSenhaAtual
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite a senha atual do usuário!")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Digite a nova senha do usuário!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
        ErrorMessage = "A senha deve ter pelo menos uma letra, um número e 6 caracteres")] 
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Confirme a nova senha  do usuário!")]
        [Compare("NovaSenha", ErrorMessage = "A nova senha e a confirmação da nova senha não coincidem!")]
        public string ConfirmarNovaSenha { get; set; }
    }
}
