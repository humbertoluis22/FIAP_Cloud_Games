using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraEstructure.Repository.Configuration
{
    public class BibliotecaConfiguration:IEntityTypeConfiguration<Biblioteca>
    {
        public void Configure(EntityTypeBuilder<Biblioteca> builder)
        {
            builder.ToTable("Biblioteca");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).HasColumnType("int").UseIdentityColumn();
            builder.Property(x => x.JogoID).IsRequired();
            builder.Property(x => x.UsuarioID).IsRequired();
            builder.Property(x => x.JogoEmprestado).IsRequired();
            builder.Property(x => x.DataAquisicao).IsRequired();

            builder.HasOne(b => b.Usuario)
                .WithMany(u => u.Bibliotecas)
                .HasForeignKey(b => b.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Jogo)
                .WithMany(j => j.Bibliotecas)
                .HasForeignKey(b => b.JogoID)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
