﻿using Core.Entity;

namespace Core.Repository
{
    public interface IBibliotecaRepository:IRepository<Biblioteca>
    {
        Task<List<Biblioteca>> ObterDadosBiblioteca();

        Task<List<Biblioteca>> ObterDadosBibliotecaUsuario(int id);

        Task<Biblioteca> RecolherJogoUsuarioId(int id, int idJogo);

        Task<List<Biblioteca>> RescolherJogosEmprestados(int idUsuario);

    }
}
