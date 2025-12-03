using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.Venda;
using System.ComponentModel.DataAnnotations;

namespace cafeservellocontroler.Models.ViewModels
{
    public class VendaViewModel
    {
        public int Id { get; set; }
        
        public int RevendedorId { get; set; }

        public List<ItemVendaViewModel> Itens { get; set; } = new List<ItemVendaViewModel> { new ItemVendaViewModel() };

        public List<ModelRevendedor> Revendedores { get; set; }
        public List<ModelProduto> Produtos { get; set; }

    }
}
