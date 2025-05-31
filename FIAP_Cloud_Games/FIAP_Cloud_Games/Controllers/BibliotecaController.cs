using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class BibliotecaController : Controller
    {
        private readonly BibliotecaAppService _bibliotecaAppService;

        public BibliotecaController(BibliotecaAppService bibliotecaAppService)
        {
            _bibliotecaAppService = bibliotecaAppService;
        }


        /// <summary>
        /// Obtém todos os dados da biblioteca de jogos.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a administradores. Retorna uma lista completa de todas as bibliotecas registradas no sistema.
        /// </remarks>
        /// <returns>Lista de bibliotecas com informações detalhadas.</returns>
        /// <response code="200">Dados retornados com sucesso</response>
        /// <response code="404">Nenhum dado encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("obterDadosBiblioteca")] // -> apenas admin
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ObterDadosBiblioteca()
        {
            try
            {
                var lista = await _bibliotecaAppService.ObterDadosBibliotecaAsync();
              
                return Ok(lista);

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
        /// Obtém os dados da biblioteca de um usuário específico.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a administradores. Retorna a lista de jogos pertencentes a um usuário pelo ID.
        /// </remarks>
        /// <param name="UsuarioId">ID do usuário.</param>
        /// <returns>Lista de jogos da biblioteca do usuário.</returns>
        /// <response code="200">Dados retornados com sucesso</response>
        /// <response code="404">Nenhum dado encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("obterDadosBibliotecaUsuario/{UsuarioId:int}")] // -> apenas admin
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ObterDadosBibliotecaUsuario([FromRoute] int UsuarioId)
        {
            try
            {
                var dadosUsuario = await _bibliotecaAppService.ObterDadosBibliotecaUsuarioAsync(UsuarioId);
                return Ok(dadosUsuario);

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
        /// Lista os jogos possuídos pelo usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <returns>Lista de jogos pertencentes ao usuário.</returns>
        /// <response code="200">Dados retornados com sucesso</response>
        /// <response code="404">Nenhum dado encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("listarJogosPossuidos")] // -> apenas user
        [Authorize(Roles = "User")]
        public async Task<ActionResult> ListarJogosPossuido()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var dadosUsuario = await _bibliotecaAppService.ObterDadosBibliotecaUsuarioAsync(userId);
             
                return Ok(dadosUsuario);

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
        /// Lista os jogos emprestados pelo usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <returns>Lista de jogos emprestados pelo usuário.</returns>
        /// <response code="200">Dados retornados com sucesso</response>
        /// <response code="404">Nenhum jogo emprestado encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("listarJogosEmprestados")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult> listarJogosEmprestados()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var jogosEmprestados = await _bibliotecaAppService.RescolherJogosEmprestadosAsync(userId);

                return Ok(jogosEmprestados);

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
        /// Adiciona um jogo à biblioteca do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <param name="jogoId">ID do jogo a ser adicionado.</param>
        /// <returns>Confirmação de criação do jogo na biblioteca.</returns>
        /// <response code="200">Jogo adicionado com sucesso</response>
        /// <response code="409">O usuário já possui este jogo</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPost("AdicionarJogo/{jogoId:int}")] // -> usuario 
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AdicionarJogo([FromRoute] int jogoId )
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
 
                await _bibliotecaAppService.AdicionarJogoAsync(userId,jogoId);

                return Ok("Jogo Adicionado com sucesso !");

            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        /// <summary>
        /// Marca um jogo como emprestado na biblioteca do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <param name="jogoId">ID do jogo a ser emprestado.</param>
        /// <returns>Confirmação de empréstimo do jogo.</returns>
        /// <response code="200">Jogo emprestado com sucesso</response>
        /// <response code="404">Jogo não encontrado para esse usuário</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPut("emprestarJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> EmprestarJogo([FromRoute] int jogoId )
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                await _bibliotecaAppService.EmprestarJogoAsync(userId, jogoId);
             
                return Ok("Jogo emprestado com sucesso!");
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
        /// Marca um jogo como devolvido na biblioteca do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <param name="jogoId">ID do jogo a ser devolvido.</param>
        /// <returns>Confirmação de devolução do jogo.</returns>
        /// <response code="200">Jogo devolvido com sucesso</response>
        /// <response code="404">Jogo não encontrado para esse usuário</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPut("devolverJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DevolverJogo([FromRoute] int jogoId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                await _bibliotecaAppService.DevolverJogoAsync(userId, jogoId);

                return Ok("Jogo devolvido com sucesso!");
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
        /// Realiza o resgate de um jogo emprestado na biblioteca do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <param name="jogoId">ID do jogo a ser resgatado do emprestimo.</param>
        /// <returns>Confirmação do resgate do jogo.</returns>
        /// <response code="200">Jogo resgatado com sucesso</response>
        /// <response code="404">Jogo não encontrado para esse usuário</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPut("solicitarJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> SolicitarJogo(int jogoId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                await _bibliotecaAppService.SolicitarJogoAsync(userId, jogoId);

                return Ok("Jogo resgatado com sucesso!");
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
        /// Remove um jogo da biblioteca do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <param name="jogoId">ID do jogo a ser removido.</param>
        /// <returns>Confirmação de remoção do jogo.</returns>
        /// <response code="200">Jogo removido com sucesso</response>
        /// <response code="404">Jogo não encontrado para esse usuário</response>
        /// <response code="400">Erro na requisição</response>
        [HttpDelete("removerJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RemoverJogo([FromRoute] int jogoId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _bibliotecaAppService.RemoverJogoAsync(userId, jogoId);

                return Ok("Jogo removido com sucesso!");
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


    }
}
