using cafeservellocontroler.Models.Pessoa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafeservellocontroler.Mapeamento
{
    public class UsuarioMap : IEntityTypeConfiguration<ModelUsuario>
    {
        public void Configure(EntityTypeBuilder<ModelUsuario> builder)
        {
            builder.ToTable("Usuario");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder
                .Property(x => x.Login)
                .HasColumnName("Login")
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Senha)
                .HasColumnName("Senha")
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Perfil)
                .HasColumnName("Perfil")
                .IsRequired();


            builder
                .Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro")
                .IsRequired();

            

        }
    }
}
