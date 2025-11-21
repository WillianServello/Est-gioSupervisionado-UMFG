using cafeservellocontroler.Enums;
using cafeservellocontroler.Mapeamento;
using cafeservellocontroler.Models;
using cafeservellocontroler.Models.Pessoa;
using cafeservellocontroler.Models.Venda;
using cafeservellocontroler.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace cafeservellocontroler.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options){ 
        }   

        public DbSet<ModelProduto> Produtos { get; set; }

        public DbSet<ModelRevendedor> Revendedor { get; set; }

        public DbSet<ModelFornecedor> Fornecedor { get; set; }

        public DbSet<ModelUsuario> Usuarios { get; set; }

        public DbSet<ModelItensVenda> ItensVendas { get; set; }

        public DbSet<ModelVenda> Vendas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           


            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new RevendedorMap());
            modelBuilder.ApplyConfiguration(new FornecedorMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ItensVendaMap());
            modelBuilder.ApplyConfiguration(new VendaMap());
        }
    }

   
}
