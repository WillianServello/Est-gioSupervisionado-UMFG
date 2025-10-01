using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models
{
    public class ModelProduto
    {   
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do produto")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite a descrição do produto")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Digite o preço do produto")]
        public decimal? Preco { get; set; }
    }
}
