using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.Login
{
    public class ModelLogin
    {
        [Required(ErrorMessage = "Por favor, informe o login!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Por favor, informe a senha!")]
        public string Senha { get; set; }
    }
}
