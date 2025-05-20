using Core.Entity;
using Core.Repository;

namespace InfraEstructure.Repository
{
    public class AdminRepository : EFRepository<Admin>,IAdminRepository
    {
        public AdminRepository(AppDbContext context) : base(context)
        {
        }
    }
}
