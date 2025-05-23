using Core.Entity;
using Core.Input.biblioteca;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace InfraEstructure.Repository
{
    public class BibliotecaRepository : EFRepository<Biblioteca>, IBibliotecaRepository
    {
        public BibliotecaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Biblioteca>> ObterDadosBiblioteca()
        {
            var lista = await _dbSet
                .Include(b => b.Jogo)
                .Include(b => b.Usuario)
                .ToListAsync();
           

            foreach (var item in lista)
            {
                if (item.Jogo != null)
                {
                    item.Jogo.Bibliotecas = null; 
                }

                if (item.Usuario != null)
                {
                    item.Usuario.Bibliotecas = null; 
                }
            }

            return lista;
        }

        public async Task<List<Biblioteca>> ObterDadosBibliotecaUsuario(int id)
        {
            var lista = await _dbSet
                 .Include(b => b.Jogo)
                 .Include(b => b.Usuario).Where(b => b.Usuario.ID == id)
                 .ToListAsync();

            foreach (var item in lista)
            {
                if (item.Jogo != null)
                {
                    item.Jogo.Bibliotecas = null;
                }

                if (item.Usuario != null)
                {
                    item.Usuario.Bibliotecas = null;
                }
            }
            return lista;
        }

        public async Task<Biblioteca> RecolherJogoUsuarioId(int idUsuario , int idJogo)
        {
            var jogo = await _dbSet
                .Where(b => b.UsuarioID == idUsuario)
                .Where(b => b.JogoID == idJogo)
                .FirstOrDefaultAsync();
            
            return jogo;
        }

        public async Task<List<Biblioteca>> RescolherJogosEmprestados(int idUsuario)
        {
            var jogosEmprestados = await _dbSet
                .Include(b => b.Jogo)
               .Where(b => b.UsuarioID == idUsuario && b.EstaEmprestado)
                .ToListAsync();

            return jogosEmprestados;
        }
    }

}
