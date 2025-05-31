using Core.Entity;
using Core.Input.biblioteca;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BibliotecaAppService
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        public BibliotecaAppService(IBibliotecaRepository bibliotecaRepository)
        {
            _bibliotecaRepository = bibliotecaRepository;
        }

        public async Task<List<BibliotecaDTO>> ObterDadosBibliotecaAsync()
        {
            var biblioteca = await _bibliotecaRepository.ObterDadosBiblioteca();
            if (biblioteca == null || !biblioteca.Any())
            {
                throw new KeyNotFoundException("Nenhum dado encontrado na biblioteca!");
            }
            return biblioteca.Select(info => new BibliotecaDTO
            {
                JogoId = info.JogoID,
                NomeJogo = info.Jogo.NomeJogo,
                Genero = info.Jogo.Genero,
                Descricao = info.Jogo.Descricao,
                Desenvolvedor = info.Jogo.Desenvolvedor,
                UsuarioId = info.UsuarioID,
                UserName = info.Usuario.UserName,
                Email = info.Usuario.Email,
                JogoEmprestado = info.JogoEmprestado,
                EstaEmprestado = info.EstaEmprestado,
                DataAquisicao = info.DataAquisicao,
            }
            ).ToList();
        }


        public async Task<List<BibliotecaDTO>> ObterDadosBibliotecaUsuarioAsync(int id)
        {
            var dadosUsuario = await _bibliotecaRepository.ObterDadosBibliotecaUsuario(id);
            if (dadosUsuario == null || !dadosUsuario.Any())
            {
                throw new KeyNotFoundException("Nenhum dado encontrado na biblioteca!");
            }

            return dadosUsuario.Select(info => new BibliotecaDTO
            {
                JogoId = info.JogoID,
                NomeJogo = info.Jogo.NomeJogo,
                Genero = info.Jogo.Genero,
                Descricao = info.Jogo.Descricao,
                Desenvolvedor = info.Jogo.Desenvolvedor,
                UsuarioId = info.UsuarioID,
                UserName = info.Usuario.UserName,
                Email = info.Usuario.Email,
                JogoEmprestado = info.JogoEmprestado,
                EstaEmprestado = info.EstaEmprestado,
                DataAquisicao = info.DataAquisicao,
            }
            ).ToList();

        }

        public async Task<List<BibliotecaJogoDTO>> RescolherJogosEmprestadosAsync(int id)
        {
            var jogosEmprestados = await _bibliotecaRepository.RescolherJogosEmprestados(id);
            if (jogosEmprestados == null || !jogosEmprestados.Any())
            {
                throw new KeyNotFoundException("Nenhum jogo emprestado encontrado!");
            }
            return jogosEmprestados.Select(info => new BibliotecaJogoDTO
            {
                JogoId = info.JogoID,
                NomeJogo = info.Jogo.NomeJogo,
                Genero = info.Jogo.Genero,
                Descricao = info.Jogo.Descricao,
                Desenvolvedor = info.Jogo.Desenvolvedor,
                JogoEmprestado = info.JogoEmprestado,
                EstaEmprestado = info.EstaEmprestado,
                DataAquisicao = info.DataAquisicao,
            }).ToList();
        }


        public async Task AdicionarJogoAsync(int userId,int jogoId)
        {
            var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);
            if (jogoBiblioteca != null)
            {
                throw new InvalidOperationException("Jogo já adicionado na biblioteca do usuário!");
            }
            var jogoNovo = new Biblioteca()
            {
                UsuarioID = userId,
                JogoID = jogoId,
            };
            await _bibliotecaRepository.CadastrarAssync(jogoNovo);
   

        }


        public async Task EmprestarJogoAsync(int userId, int jogoId ) 
        {
            var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);
            if (jogoBiblioteca == null)
            {
               throw new KeyNotFoundException("Nenhum jogo encontrado para esse usuário!");
            }

            jogoBiblioteca.EmprestarJogo();

            await _bibliotecaRepository.AlterarAsync(jogoBiblioteca);

        }


        public async Task DevolverJogoAsync(int userId, int jogoId)
        {
            var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);
            if (jogoBiblioteca == null)
            {
                throw new KeyNotFoundException("Nenhum jogo encontrado para esse usuário!");
            }

            jogoBiblioteca.DevolverJogo();

            await _bibliotecaRepository.AlterarAsync(jogoBiblioteca);
  
        }


        public async Task SolicitarJogoAsync(int userId, int jogoId)
        {
            var jogolib = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);

            if (jogolib == null)
            {
                throw new KeyNotFoundException("Nenhum jogo encontrado para esse usuário!");
            }

            jogolib.SolicitarJogo();
            await _bibliotecaRepository.AlterarAsync(jogolib);

        }


        public async Task RemoverJogoAsync(int userId, int jogoId)
        {
            var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);

            if (jogoBiblioteca == null)
            {
                  throw new KeyNotFoundException("Nenhum jogo encontrado para esse usuário!");
            }
            await _bibliotecaRepository.DeletarAsync(jogoBiblioteca.ID);

        }


    }
}
