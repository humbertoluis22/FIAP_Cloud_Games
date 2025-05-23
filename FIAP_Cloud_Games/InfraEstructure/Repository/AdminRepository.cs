using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace InfraEstructure.Repository
{
    public class AdminRepository : EFRepository<Admin>,IAdminRepository
    {
        public AdminRepository(AppDbContext context) : base(context)
        {
        }

        
        public async Task<List<Admin>> ListarAdminsPorEmailouUserName(string email, string userName)
        {
            var admins = await _dbSet.Where(a => a.Email == email || a.UserName == userName).ToListAsync();
            return admins;
        }


        public async Task<Admin> ObterPorLogin(string username, string email, string senha)
        {
             var usuario = await _dbSet.FirstOrDefaultAsync(u => u.UserName == username && u.Email == email && u.Senha == senha);
             return usuario;

        }
    }
}
