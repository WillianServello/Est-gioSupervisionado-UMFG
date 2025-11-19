using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.Login
{
    public class ModelRecuperarSenha
    {
        [Required(ErrorMessage = "O login é obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório")]
        public string Email { get; set; }
    }
}
