using Core.Entity;

namespace Core.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<List<T>> ObterTodosAsync();
        Task<T> ObterPorIdAsync(int ID);
        Task CadastrarAssync(T entity);
        Task DeletarAsync(int ID);
        Task<T> AlterarAsync(T entity);
    }
}
