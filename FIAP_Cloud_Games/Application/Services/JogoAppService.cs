using Core.Entity;
using Core.Input.jogo;
using Core.Input;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JogoAppService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoAppService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<List<JogoDto>> ListarJogosAsync()
        {
            var jogos = await _jogoRepository.ObterTodosAsync();
            if (jogos == null || !jogos.Any())
            {
                throw new KeyNotFoundException("Nenhum jogo encontrado!");
            }
            return jogos.Select(jogo => new JogoDto
            {
                JogoId = jogo.ID,
                NomeJogo = jogo.NomeJogo,
                Genero = jogo.Genero,
                Descricao = jogo.Descricao,
                Desenvolvedor = jogo.Desenvolvedor,
                Preco = jogo.Preco
            }).ToList();
        }

        public async Task<JogoDto?> ObterJogoPorIdAsync(int id)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo == null)
            {
                throw new KeyNotFoundException("Nenhum jogo encontrado!");
            }

            return new JogoDto
            {
                JogoId = jogo.ID,
                NomeJogo = jogo.NomeJogo,
                Genero = jogo.Genero,
                Descricao = jogo.Descricao,
                Desenvolvedor = jogo.Desenvolvedor,
                Preco = jogo.Preco
            };
        }

        public async Task<JogoDto> CriarJogoAsync(JogoInput input)
        {
            var existente = await _jogoRepository
                .ListarJogosPorNome(input.NomeJogo);

            if (existente != null)
                throw new InvalidOperationException("Jogo com este nome já existe!");
            var jogo = new Jogo(
                input.IdAdmin,
                input.NomeJogo,
                input.Genero,
                input.Descricao,
                input.Desenvolvedor,
                input.Preco
            );

           
            await _jogoRepository.CadastrarAssync(jogo);

            return new JogoDto
            {
                JogoId = jogo.ID,
                NomeJogo = jogo.NomeJogo,
                Genero = jogo.Genero,
                Descricao = jogo.Descricao,
                Desenvolvedor = jogo.Desenvolvedor,
                Preco = jogo.Preco
            };
        }

        public async Task<JogoDto?> AtualizarJogoAsync(JogoUpdateInput input)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(input.JogoID);
            if (jogo == null)
            {
                throw new KeyNotFoundException("Jogo não encontrado");
            }

            jogo.NomeJogo = input.NomeJogo;
            jogo.Genero = input.Genero;
            jogo.Descricao = input.Descricao;
            jogo.Preco = input.Preco;

            await _jogoRepository.AlterarAsync(jogo);

            return new JogoDto
            {
                JogoId = jogo.ID,
                NomeJogo = jogo.NomeJogo,
                Genero = jogo.Genero,
                Descricao = jogo.Descricao,
                Desenvolvedor = jogo.Desenvolvedor,
                Preco = jogo.Preco
            };
        }

        public async Task<bool> DeletarJogoAsync(int id)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo == null)
            {
                throw new KeyNotFoundException("Jogo não encontrado");
            };

            await _jogoRepository.DeletarAsync(jogo.ID);
            return true;
        }
    }
}
