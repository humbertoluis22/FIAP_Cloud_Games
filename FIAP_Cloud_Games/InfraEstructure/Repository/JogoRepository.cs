using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace InfraEstructure.Repository
{
    public class JogoRepository : EFRepository<Jogo>, IJogoRepository
    {
        public JogoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Jogo> ListarJogosPorNome(string n)
        {
            var jogo = await _dbSet.FirstOrDefaultAsync(j => j.NomeJogo == n);
            return jogo;
        }
    }

}
