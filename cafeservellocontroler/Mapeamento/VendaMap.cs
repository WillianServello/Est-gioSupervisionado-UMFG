using cafeservellocontroler.Models.Venda;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafeservellocontroler.Mapeamento
{
    public class VendaMap : IEntityTypeConfiguration<ModelVenda>
    {
        public void Configure(EntityTypeBuilder<ModelVenda> builder)
        {
            builder.ToTable("Vendas");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.DataVenda)
                .IsRequired();

            builder.Ignore(x => x.TotalVenda);

            builder
                .HasOne(v => v.Usuario)
                .WithMany()
                .HasForeignKey("Id_Usuario")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(v => v.Revendedor)
                .WithMany()
                .HasForeignKey("Id_Revendedor")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasMany(v => v.ItensVendas)
                .WithOne()
                .HasForeignKey("Id_Venda")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
