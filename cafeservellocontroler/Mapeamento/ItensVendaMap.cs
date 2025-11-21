using cafeservellocontroler.Models.Venda;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafeservellocontroler.Mapeamento
{
    public class ItensVendaMap : IEntityTypeConfiguration<ModelItensVenda>
    {
        public void Configure(EntityTypeBuilder<ModelItensVenda> builder)
        {
            builder.ToTable("ItensVenda");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantidade)
                .HasColumnName("Quantidade")
                .IsRequired();

            builder.Property(x => x.Valor)
                .HasColumnName("Valor")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Total)
                .HasColumnName("Total")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .HasOne(iv => iv.Produto)
                .WithMany()
                .HasForeignKey("Id_Produto")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
