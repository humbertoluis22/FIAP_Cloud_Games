using Core.Entity;

namespace Core.Repository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Task<Admin> ObterPorLogin(string username,string email,string senha);

        Task<List<Admin>> ListarAdminsPorEmailouUserName(string email,string userName);

    }
}
