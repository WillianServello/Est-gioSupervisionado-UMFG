using cafeservellocontroler.Models;
using Microsoft.EntityFrameworkCore;

namespace cafeservellocontroler.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options){ 
        }   

        public DbSet<ModelProduto> Produtos { get; set; }
    }
}
