using Core.Entity;
using Core.Repository;

namespace InfraEstructure.Repository
{
    public class BibliotecaRepository : EFRepository<Biblioteca>, IBibliotecaRepository
    {
        public BibliotecaRepository(AppDbContext context) : base(context)
        {
        }
    }

}
