using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraEstructure.Repository.Configuration
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admin");
            builder.HasKey(a => a.ID);
            builder.Property(a => a.ID).HasColumnType("int").IsRequired();
            builder.Property(a => a.UserName).HasColumnType("varchar(50)").IsRequired();
            builder.Property(a => a.Senha).HasColumnType("varchar(50)").IsRequired();
            builder.Property(a => a.Email).HasColumnType("varchar(50)").IsRequired();
            builder.Property(a => a.DataCriacao).HasColumnType("datetime").IsRequired();

            builder.HasMany(a =>a.Jogos)
                .WithOne(j => j.Admin)
                .HasForeignKey(j => j.IdAdmin)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
