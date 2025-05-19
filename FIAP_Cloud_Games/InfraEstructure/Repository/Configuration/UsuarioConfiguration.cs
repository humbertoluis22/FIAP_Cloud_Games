using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraEstructure.Repository.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(u => u.ID);
            builder.Property(u => u.ID).HasColumnType("int").IsRequired();
            builder.Property(u => u.UserName).HasColumnType("varchar(50)").IsRequired();
            builder.Property(u => u.Senha).HasColumnType("varchar(50)").IsRequired();
            builder.Property(u => u.Email).HasColumnType("varchar(50)").IsRequired();
            builder.Property(u => u.DataInscricao).HasColumnType("datetime").IsRequired();
            builder.Property(u => u.TentativasErradas).HasColumnType("int").IsRequired();
            builder.Property(u => u.Bloqueado).HasColumnType("bit").IsRequired();

            builder.HasMany(u => u.Bibliotecas)
                .WithOne(b => b.Usuario)
                .HasForeignKey(b => b.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

