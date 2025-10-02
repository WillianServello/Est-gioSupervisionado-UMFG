using cafeservellocontroler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafeservellocontroler.Mapeamento
{
    public class ProdutoMap : IEntityTypeConfiguration<ModelProduto>
    {
        public void Configure(EntityTypeBuilder<ModelProduto> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("ID");

            builder
                .Property(x => x.Nome)
                .HasColumnName("NomeProduto")
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(x => x.Descricao)
                .HasColumnName("DescricaoProduto")
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(x => x.Preco)
                .HasColumnName("PrecoProduto")
                .IsRequired();

            builder
                .Property(x => x.Estoque)
                .HasColumnName("EstoqueProduto")
                .IsRequired();
        }
    }
}
