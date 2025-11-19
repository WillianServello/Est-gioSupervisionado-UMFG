using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.Login
{
    public class ModelLogin
    {
        [Required(ErrorMessage = "O login é obrigatório")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A senha é obrigatório")]
        public string Senha { get; set; }
    }
}
