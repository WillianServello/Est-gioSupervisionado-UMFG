using cafeservellocontroler.Models.Pessoa;

namespace cafeservellocontroler.Models.Venda
{
    public class ModelVenda
    {
        public int Id { get; private set; }

        public DateTime DataVenda { get; private set; } = DateTime.Now;

        public DateTime DataAtualizarVenda { get; private set; } = DateTime.Now;


        //protagonistas da venda 
        public ModelUsuario Usuario { get; private set; }

        public ModelRevendedor Revendedor { get; private set; }

        public ICollection<ModelItensVenda> ItensVendas { get; private set; } = [];



        //calcula o total da venda somando o total de cada item
        public decimal TotalVenda => ItensVendas.Sum(item => item.Total);




        //construtor da venda recebendo o usuario e o revendedor
        public ModelVenda(ModelUsuario usuario, ModelRevendedor revendedor)
        {
            Usuario = usuario;
            Revendedor = revendedor;
        }

        public ModelVenda()
        {
        }


        //adicionar item na venda, recebendo o produto e a quantidade que o usuario escolher
        public void AdicionarItem(ModelProduto produto, int quantidade)
        {
            var item = new ModelItensVenda(produto, quantidade);
            ItensVendas.Add(item);
        }

        public void AtualizarVenda(ModelUsuario usuario, ModelRevendedor revendedor)
        {
            Usuario = usuario;
            Revendedor = revendedor;
        }

        public void AtualizarDataVenda()
        {
            DataAtualizarVenda = DateTime.Now;
        }
    }


}

