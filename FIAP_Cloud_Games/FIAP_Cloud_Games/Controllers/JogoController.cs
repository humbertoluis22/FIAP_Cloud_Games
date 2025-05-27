using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Core.Input.jogo;
using Core.Entity;
using Microsoft.AspNetCore.Authorization;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class JogoController : Controller
    {
        private readonly IJogoRepository _jogoRepository;
        public JogoController(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }


        /// <summary>
        /// Lista todos os jogos cadastrados.
        /// </summary>
        /// <remarks>
        /// Retorna a lista completa de jogos disponíveis no sistema.
        /// Acesso permitido a todos os usuários, autenticados ou não.
        /// </remarks>
        /// <returns>Lista de jogos cadastrados.</returns>
        /// <response code="200">Lista de jogos retornada com sucesso.</response>
        /// <response code="404">Nenhum jogo encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpGet("listarJogo")]
        [AllowAnonymous]
        public async Task<ActionResult> ListarJogo()
        {
            try
            {
                var lista = await _jogoRepository.ObterTodosAsync();
                if (lista == null || lista.Count == 0)
                {
                    return NotFound("Nenhum jogo encontrado!");
                }

                var listaDTO = new List<JogoDto>();
                foreach (var jogo in lista)
                {
                    listaDTO.Add(
                        new JogoDto()
                        {
                            JogoId = jogo.ID,
                            NomeJogo = jogo.NomeJogo,
                            Genero = jogo.Genero,
                            Descricao = jogo.Descricao,
                            Desenvolvedor = jogo.Desenvolvedor,
                            Preco = jogo.Preco,

                        });
                }
                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Recupera um jogo específico pelo ID.
        /// </summary>
        /// <remarks>
        /// Retorna os detalhes de um jogo específico através do seu identificador.
        /// Acesso permitido a todos os usuários, autenticados ou não.
        /// </remarks>
        /// <param name="id">ID do jogo a ser consultado.</param>
        /// <returns>Detalhes do jogo solicitado.</returns>
        /// <response code="200">Jogo encontrado com sucesso.</response>
        /// <response code="404">Nenhum jogo encontrado com o ID informado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpGet("recolherJogoId/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> RecolherJogoId(int id)
        {
            try
            {
                var jogo = await _jogoRepository.ObterPorIdAsync(id);
                if (jogo == null)
                {
                    return NotFound("Nenhum jogo encontrado!");
                }
                var jogoDTO = new JogoDto()
                {
                    JogoId = jogo.ID,
                    NomeJogo = jogo.NomeJogo,
                    Genero = jogo.Genero,
                    Descricao = jogo.Descricao,
                    Desenvolvedor = jogo.Desenvolvedor,
                    Preco = jogo.Preco,
                };
                return Ok(jogoDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Cadastra um novo jogo.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a Admin
        /// </remarks>
        /// <param name="jogoInput">Dados do jogo a ser cadastrado.</param>
        /// <returns>Confirmação da criação do jogo com os dados cadastrados.</returns>
        /// <response code="201">Jogo criado com sucesso.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpPost("criarJogo")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CriarJogo([FromBody] JogoInput jogoInput)
        {
            try
            {
                Jogo jogo = new Jogo(
                    jogoInput.IdAdmin,
                    jogoInput.NomeJogo,
                    jogoInput.Genero,
                    jogoInput.Descricao,
                    jogoInput.Desenvolvedor,
                    jogoInput.Preco);

                await _jogoRepository.CadastrarAssync(jogo);

                var jogoDTO = new JogoDto()
                {
                    JogoId = jogo.ID,
                    NomeJogo = jogo.NomeJogo,
                    Genero = jogo.Genero,
                    Descricao = jogo.Descricao,
                    Desenvolvedor = jogo.Desenvolvedor,
                    Preco = jogo.Preco,
                };

                return Created("Jogo criado com sucesso !", jogoDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Atualiza os dados de um jogo existente.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a Admin
        /// </remarks>
        /// <param name="jogoInput">Dados atualizados do jogo.</param>
        /// <returns>Confirmação da atualização com os dados atualizados do jogo.</returns>
        /// <response code="200">Jogo atualizado com sucesso.</response>
        /// <response code="404">Jogo não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpPut("atualizarJogo")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AtualizarJogo([FromBody] JogoUpdateInput jogoInput)
        {
            try
            {

                var jogo = await _jogoRepository.ObterPorIdAsync(jogoInput.JogoID);
                
                if (jogo == null)
                {
                    return NotFound("Jogo não encontrado!");
                }   

                jogo.NomeJogo = jogoInput.NomeJogo;
                jogo.Genero = jogoInput.Genero;
                jogo.Descricao = jogoInput.Descricao;
                jogo.Preco = jogoInput.Preco;

                await _jogoRepository.AlterarAsync(jogo);

                var jogoDTO = new JogoDto()
                {
                    JogoId = jogo.ID,
                    NomeJogo = jogo.NomeJogo,
                    Genero = jogo.Genero,
                    Descricao = jogo.Descricao,
                    Desenvolvedor = jogo.Desenvolvedor,
                    Preco = jogo.Preco,
                };

                return Ok(new
                {
                    Messagem = "Jogo atualizado com sucesso !",
                    Dados = jogoDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        /// <summary>
        /// Remove um jogo do sistema.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a Admin.
        /// </remarks>
        /// <param name="id">ID do jogo a ser removido.</param>
        /// <returns>Confirmação da remoção do jogo.</returns>
        /// <response code="200">Jogo deletado com sucesso.</response>
        /// <response code="404">Jogo não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpDelete("deletarJogo/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletarJogo(int id)
        {
            try
            {
                var jogo = await _jogoRepository.ObterPorIdAsync(id);
                if (jogo == null)
                {
                    return NotFound("Jogo não encontrado!");
                }
                await _jogoRepository.DeletarAsync(jogo.ID);
                return Ok("Jogo deletado com sucesso !");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    
    
    }
}
