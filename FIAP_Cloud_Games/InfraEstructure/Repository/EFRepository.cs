using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace InfraEstructure.Repository
{
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EFRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task CadastrarAssync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> ObterPorIdAsync(int ID)
        => await  _dbSet.FirstOrDefaultAsync(x => x.ID == ID);
          
        

        public async Task<List<T>> ObterTodosAsync()
        => await _dbSet.ToListAsync();
        

        public async Task DeletarAsync(int ID)
        {
            await _dbSet.Where(t => t.ID == ID).ExecuteDeleteAsync();
        }


        public async Task AlterarAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }


    }
}
