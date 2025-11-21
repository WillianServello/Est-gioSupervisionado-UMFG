using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.Venda
{
    public class ModelItensVenda
    {

        public int Id { get; private set; }

        [Required(ErrorMessage = "Informe o produto")]
        public ModelProduto Produto { get; private set; }
        public decimal Valor { get; private set; } = decimal.Zero;

        [Required(ErrorMessage = "Informe uma quantidade")]
        public int Quantidade { get; private set; }
        public decimal Total { get; private set; } = decimal.Zero;

        public ModelItensVenda(ModelProduto modelProduto, int quantidade)
        {
            Produto = modelProduto;
            Valor = modelProduto.Preco;
            Quantidade = quantidade;
            Total = Valor * Quantidade;
        }

        private ModelItensVenda()
        {
        }


        //Atualizar quantidade do produto na venda

        public void AtualizarQuantidade(int novaQuantidade)
        {
            Quantidade = novaQuantidade;
            Total = Valor * Quantidade;
        }
    }
}
