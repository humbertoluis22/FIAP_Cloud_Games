﻿using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraEstructure.Repository.Configuration
{
    public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("Jogo");
            builder.HasKey(j => j.ID);
            builder.Property(j => j.ID).HasColumnType("int").UseIdentityColumn();
            builder.Property(j => j.NomeJogo).HasColumnType("varchar(50)").IsRequired();
            builder.Property(j => j.Genero).HasColumnType("varchar(20)").IsRequired();
            builder.Property(j => j.IdAdmin).HasColumnType("int").IsRequired();
            builder.Property(j => j.DataCriacao).HasColumnType("datetime").IsRequired();
            builder.Property(j => j.Descricao).HasColumnType("varchar(200)").IsRequired();
            builder.Property(j => j.Desenvolvedor).HasColumnType("varchar(50)").IsRequired();
            builder.Property(j => j.Preco).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasMany(j => j.Bibliotecas)
                .WithOne(b => b.Jogo)
                .HasForeignKey(b => b.JogoID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.Admin)
                .WithMany(a => a.Jogos)
                .HasForeignKey(j => j.IdAdmin)
                .OnDelete(DeleteBehavior.Restrict); 
        }


    }
}

