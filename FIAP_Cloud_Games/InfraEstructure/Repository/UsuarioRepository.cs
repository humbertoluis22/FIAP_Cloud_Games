using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace InfraEstructure.Repository
{
    public class UsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context)
        {
        }



        public async Task<Usuario> ObterPorLogin(string userName, string email, string senha)
        {
             var usuario = await  _dbSet.FirstOrDefaultAsync(u => u.UserName == userName && u.Email == email && u.Senha == senha);
            return usuario;
        }

        public async Task<ICollection<Usuario>> RecolherUsuarioBloqueados()
        {
             var usuarios = await _dbSet.Where(x => x.Bloqueado == true).ToListAsync();
             return usuarios;
        }

        public async Task<Usuario> recolherusuarioPorEmailUsername(string userName, string email)
        {
            var usuario = await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName && u.Email == email);
            return usuario;
        }


        public async Task<ICollection<Usuario>> ListarUsuarioPorEmailouUserName(string userName, string email)
        {
            var usuarios = await _dbSet.
                Where(a => a.Email == email || a.UserName == userName)
                .ToListAsync();

            return usuarios;
        }
    }

}
