using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace InfraEstructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }

}
