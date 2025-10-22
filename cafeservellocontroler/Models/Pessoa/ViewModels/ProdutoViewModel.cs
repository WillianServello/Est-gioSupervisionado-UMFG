using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.Pessoa.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? Preco { get; set; }
        public int? Estoque { get; set; } 
    }
}
