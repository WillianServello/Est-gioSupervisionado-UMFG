using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cafeservellocontroler.Models.ViewModels
{
    public class ProdutoViewModel
    {
        //objetivo de amnha fazer as validacoes de entrada, required, format, length, etc

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do produto")]
        [MaxLength(100, ErrorMessage = "Atingiu o limite de caracteres")]
        [MinLength(3, ErrorMessage = "No minimo o nome deve ter 3 letras!")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "Use apenas letras.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Digite uma descrição")]
        [MaxLength(255, ErrorMessage = "Atingiu o limite de caracteres")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Digite o preço de compra")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? PrecoCompra { get; set; }

        [Required(ErrorMessage = "Digite um preço")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "Digite um estoque disponivel")]
        [Range(0, int.MaxValue, ErrorMessage = "O valor não pode ser negativo.")]
        public int? Estoque { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime DataAtualizacaoCadastro { get; set; } = DateTime.Now;

    }
}
