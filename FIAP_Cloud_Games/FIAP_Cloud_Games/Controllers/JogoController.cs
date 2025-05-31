using Application.Services;
using Core.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class JogoController : Controller
    {
        private readonly JogoAppService _jogoAppService;
        public JogoController(JogoAppService jogoAppService)
        {
            _jogoAppService = jogoAppService;
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
                var jogos = await _jogoAppService.ListarJogosAsync();

                return Ok(jogos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                var jogo = await _jogoAppService.ObterJogoPorIdAsync(id);
                return Ok(jogo);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                var jogo = await _jogoAppService.CriarJogoAsync(jogoInput);
                return Created("Jogo criado com sucesso!", jogo);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
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

                var jogo = await _jogoAppService.AtualizarJogoAsync(jogoInput);
         
                return Ok(new
                {
                    Mensagem = "Jogo atualizado com sucesso!",
                    Dados = jogo
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                var sucesso = await _jogoAppService.DeletarJogoAsync(id);
          
                return Ok("Jogo deletado com sucesso!");
            }
            catch (KeyNotFoundException  ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
