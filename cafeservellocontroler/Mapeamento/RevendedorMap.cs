using cafeservellocontroler.Models.Pessoa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafeservellocontroler.Mapeamento
{
    public class RevendedorMap : IEntityTypeConfiguration<ModelRevendedor>
    {
        public void Configure(EntityTypeBuilder<ModelRevendedor> builder)
        {
            builder.ToTable("Revendedor");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder 
                .Property(x => x.Nome)
                .HasColumnName("Nome")
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(x => x.Telefone)
                .HasColumnName("Telefone")
                .HasMaxLength(15)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Cnpj)
                .HasColumnName("Cnpj")
                .HasMaxLength(18)
                .IsRequired();

            builder
                .Property(x => x.Endereco)
                .HasColumnName("Endereco")
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(x => x.NomeFantasia)
                .HasColumnName("NomeFantasia")
                .HasMaxLength(100)
                .IsRequired();

        }
    }
}
