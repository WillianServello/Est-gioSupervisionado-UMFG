using cafeservellocontroler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafeservellocontroler.Mapeamento
{
    public class ProdutoMap : IEntityTypeConfiguration<ModelProduto>
    {
        public void Configure(EntityTypeBuilder<ModelProduto> builder)
        {
            builder.ToTable("Produtos");

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

            builder.Property(x => x.PrecoCompra)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .Property(x => x.Preco)
                .HasColumnName("PrecoProduto")
                .HasColumnType("numeric(10, 2)")
                .IsRequired();

            builder
                .Property(x => x.Estoque)
                .HasColumnName("EstoqueProduto")
                .IsRequired();

            builder
                .Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro")
                .IsRequired();

            builder
                .Property(x => x.DataAtualizacaoCadastro)
                .HasColumnName("DataAtualizacaoCadastro")
                .IsRequired();
        }
    }
}
