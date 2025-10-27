using cafeservellocontroler.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models
{
    public class ModelProduto
    {   
        public int Id { get; set; } 
        [Required(ErrorMessage = "Digite o nome do produto")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite a descrição do produto")]
        public string Descricao { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite o preço do produto")]

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Preco { get; set; } = decimal.Zero;

        public int Estoque { get; set; }





        public ModelProduto(string nome, decimal preco, int estoque)
        {

            Nome = nome;
            Preco = preco;
            Estoque = estoque;
        }

        public ModelProduto()
        {
        }

        public void AtualizarDados(ProdutoViewModel viewModel)
        {
            Nome = viewModel.Nome;
            Descricao = viewModel.Descricao;
            Preco = viewModel.Preco ?? 0;
            Estoque = viewModel.Estoque ?? 0;
        }
    }
}
