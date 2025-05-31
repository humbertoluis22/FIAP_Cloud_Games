using Application.Services;
using Core.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UserAppService _userAppService;
        public UsuarioController(UserAppService userAppService)
        {
            _userAppService = userAppService;
        }


        /// <summary>
        /// Lista todos os usuários cadastrados.
        /// </summary>
        /// <remarks>
        /// Acesso permitido a todos os usuários, autenticados ou não.
        /// </remarks>
        /// <returns>Lista de usuários cadastrados.</returns>
        /// <response code="200">Lista de usuários retornada com sucesso.</response>
        /// <response code="404">Nenhum usuário encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpGet("listarUsuarios")]
        [AllowAnonymous]
        public async Task<ActionResult> ListarUsuarios()
        {
            try
            {
                var usuarios = await _userAppService.ListarTodosAsync();
                if (!usuarios.Any())
                {
                    return NotFound("Nenhum usuario encontrado !");
                }
               
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Lista todos os usuários bloqueados.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a Admin.
        /// </remarks>
        /// <returns>Lista de usuários bloqueados.</returns>
        /// <response code="200">Lista de usuários bloqueados retornada com sucesso.</response>
        /// <response code="404">Nenhum usuário bloqueado encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpGet("listarUsuariosBloqueado")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RecolherUsuarioBloqueados()
        {
            try
            {
                var usuariosBloqueados = await _userAppService.ListarBloqueadosAsync();
                if (!usuariosBloqueados.Any())
                {
                    return NotFound("Nenhum usuario bloqueado encontrado !");
                }

                return Ok(usuariosBloqueados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        /// <remarks>
        /// Acesso permitido a todos os usuários, autenticados ou não.
        /// </remarks>
        /// <param name="usuarioInput">Dados do usuário a ser cadastrado.</param>
        /// <returns>Confirmação da criação do usuário com os dados cadastrados.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="409">UserName ou Email já utilizados.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpPost("cadastrarUsuario")]
        [AllowAnonymous]
        public async Task<ActionResult> CadastrarUsuario([FromBody] UsuarioInput usuarioInput)
        {
            try
            {
                var usuario = await _userAppService.CadastrarUsuarioAsync(usuarioInput);
                return Created("Usuario criado com sucesso!", usuario);

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



        // <summary>
        /// Altera a senha do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User" ou "Admin".
        /// </remarks>
        /// <param name="usuarioInput">Dados para atualização da senha.</param>
        /// <returns>Confirmação da alteração da senha.</returns>
        /// <response code="200">Senha alterada com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpPut("alterarSenha")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> AlterarSenha([FromBody] UsuarioUpdateInput usuarioInput)
        {
            try
            {
                var usuario = await _userAppService.AlterarSenhaAsync(usuarioInput);
                return Ok(usuario);
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
        /// Altera o e-mail do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User" ou "Admin".
        /// </remarks>
        /// <param name="usuarioInput">Dados para atualização do e-mail.</param>
        /// <returns>Confirmação da alteração do e-mail.</returns>
        /// <response code="200">E-mail alterado com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpPut("AlterarEmail")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> AlterarEmail(
            [FromBody] UsuarioUpdateEmailInput usuarioInput
            )
        {
            try
            {
                var usuario = await _userAppService.AlterarEmailAsync(usuarioInput);
                return Ok(usuario);
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
        /// Bloqueia um usuário específico.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a Admin.
        /// </remarks>
        /// <param name="usuarioId">ID do usuário a ser bloqueado.</param>
        /// <returns>Confirmação do bloqueio do usuário.</returns>
        /// <response code="200">Usuário bloqueado com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpPut("bloquearUsuario/{usuarioId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> BloquarUsuario(int usuarioId)
        {
            try
            {
                await _userAppService.BloquearUsuarioAsync(usuarioId);
                return Ok("Usuário bloqueado com sucesso");
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
        /// Desbloqueia um usuário específico.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a Admin.
        /// </remarks>
        /// <param name="usuarioId">ID do usuário a ser desbloqueado.</param>
        /// <returns>Confirmação do desbloqueio do usuário.</returns>
        /// <response code="200">Usuário desbloqueado com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpPut("desbloquearUsuario/{usuarioId}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> DesbloquearUsuario(int usuarioId)
        {
            try
            {
                await _userAppService.DesbloquearUsuarioAsync(usuarioId);
                return Ok("Usuário desbloqueado com sucesso");
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
        /// Remove um usuário do sistema.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a Admin.
        /// </remarks>
        /// <param name="usuarioId">ID do usuário a ser removido.</param>
        /// <returns>Confirmação da remoção do usuário.</returns>
        /// <response code="200">Usuário deletado com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpDelete("deletarUsuario/{usuarioId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletarUsuario(int usuarioId)
        {
            try
            {
                await _userAppService.DeletarUsuarioAsync(usuarioId);
                return Ok("Usuário deletado com sucesso");

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
        /// Remove a conta do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Acesso restrito a usuários autenticados com perfil "User".
        /// </remarks>
        /// <returns>Confirmação da remoção da conta.</returns>
        /// <response code="200">Conta deletada com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpDelete("deletarConta")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeletarConta()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                await _userAppService.DeletarUsuarioAsync(userId);
                return Ok("Usuário deletado com sucesso");
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
