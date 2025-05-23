using Core.Entity;

namespace Core.Repository
{
    public interface IUsuarioRepository:IRepository<Usuario>
    {

        Task<ICollection<Usuario>> RecolherUsuarioBloqueados();

        Task<Usuario> ObterPorLogin(string userName,string email,string senha);

        Task<Usuario> recolherusuarioPorEmailUsername(string userName, string email);

        Task<ICollection<Usuario>> ListarUsuarioPorEmailouUserName(string userName, string email);
    }
}
