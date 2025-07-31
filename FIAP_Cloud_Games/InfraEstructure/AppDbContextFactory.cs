using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InfraEstructure
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("Executando em tempo de design");
            
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

                //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;" +
                //    "Database=FiapCloudGames;" +
                //    "Integrated Security=True;" +
                //    "Connect Timeout=30;" +
                //    "Encrypt=False;" +
                //    "TrustServerCertificate=False;" +
                //    "Application Intent=ReadWrite;" +
                //    "Multi Subnet Failover=False");
                //optionsBuilder.UseSqlServer("Server=localhost,1433;Database=FiapCloudGames;User Id=sa;Password=SuaSenhaForte#123;TrustServerCertificate=True;");

                optionsBuilder.UseSqlServer("Server=0.tcp.sa.ngrok.io,11340;Database=FiapCloudGames;User Id=sa;Password=SuaSenhaForte#123;TrustServerCertificate=True;");
                return new AppDbContext(optionsBuilder.Options);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ocorreu um  erro ao se conectar  com o banco  : " + ex.Message);
                throw;
            }         
        }
    }

}
