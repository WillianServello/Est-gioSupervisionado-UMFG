namespace cafeservellocontroler.Models.ViewModels
{
    public class ItemVendaViewModel
    {
        public int Id { get; set; }

        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }

        // Valor unitário do produto
        public decimal Valor { get; set; } = 0;

        // Total do item (Valor * Quantidade)
        public decimal Total => Valor * Quantidade;

        public decimal PrecoProduto { get; set; }
        public int EstoqueProduto { get; set; }
    }
}
