using Core.Entity;
using Core.Repository;

namespace InfraEstructure.Repository
{
    public class JogoRepository : EFRepository<Jogo>, IJogoRepository
    {
        public JogoRepository(AppDbContext context) : base(context)
        {
        }
    }

}
