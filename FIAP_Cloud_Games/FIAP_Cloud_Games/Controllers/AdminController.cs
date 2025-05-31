using Application.Services;
using Core.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP_Cloud_Games.Controllers
{

    [Controller]
    [Route("v1/[controller]")]
    public class AdminController : Controller
    {
        private readonly AdminAppService _adminAppService;
        public AdminController(AdminAppService adminAppService)
        {
            _adminAppService = adminAppService;
        }


        /// <summary>
        /// Lista todos os administradores cadastrados.
        /// </summary>
        /// <remarks>
        /// Requisição pública para obter todos os administradores disponíveis no sistema.
        /// Não exige autenticação.
        /// </remarks>
        /// <returns>Retorna uma lista de administradores no formato DTO.</returns>
        /// <response code="200">Retorna a lista de administradores</response>
        /// <response code="404">Nenhum administrador encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("listarAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult> ListarAdmin()
        {
            try
            {
                var admins = await _adminAppService.ListarTodosAsync();

                if (!admins.Any())
                    return NotFound("Nenhum admin encontrado!");

                return Ok(admins);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Retorna um administrador pelo ID.
        /// </summary>
        /// <remarks>
        /// Requer autenticação com perfil de Admin.
        /// </remarks>
        /// <param name="id">ID do administrador.</param>
        /// <returns>Dados do administrador.</returns>
        /// <response code="200">Retorna o administrador encontrado</response>
        /// <response code="404">Administrador não encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("recolherAdminid/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RecolherAdmin([FromRoute]int id)
        {
            try
            {
                var admin = await _adminAppService.ObterPorIdAsync(id);
                if (admin == null)
                    return NotFound("Admin não encontrado!");

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Cria um novo administrador.
        /// </summary>
        /// <remarks>
        /// Requisição pública para criação de um novo administrador.
        /// </remarks>
        /// <param name="adminInput">Dados de entrada do administrador.</param>
        /// <returns>Administrador criado no formato DTO.</returns>
        /// <response code="201">Administrador criado com sucesso</response>
        /// <response code="409">Username ou Email já existe</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPost("criarAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult> CriarAdmin([FromBody] AdminInput adminInput )
        {
            try
            {
                var adminDTO = await _adminAppService.CriarAsync(adminInput);
                return Created("Admin criado com sucesso!", adminDTO);

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
        /// Atualiza os dados de um administrador existente.
        /// </summary>
        /// <remarks>
        /// Requer autenticação com perfil de Admin.
        /// </remarks>
        /// <param name="adminUpdateInput">Dados atualizados do administrador.</param>
        /// <returns>Confirmação da atualização e dados atualizados.</returns>
        /// <response code="200">Administrador atualizado com sucesso</response>
        /// <response code="404">Administrador não encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPut("atualizarAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AtualizarAdmin([FromBody] AdminUpdateInput adminUpdateInput) 
        {
            try 
            {
                var adminDTO = await _adminAppService.AtualizarAsync(adminUpdateInput);
                if (adminDTO == null)
                    return NotFound("Admin não encontrado!");

                return Ok(new
                {
                    Mensagem = "Admin atualizado com sucesso!",
                    Dados = adminDTO
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Deleta um administrador pelo ID.
        /// </summary>
        /// <remarks>
        /// Requer autenticação com perfil de Admin.
        /// </remarks>
        /// <param name="id">ID do administrador a ser deletado.</param>
        /// <returns>Confirmação da exclusão.</returns>
        /// <response code="200">Administrador deletado com sucesso</response>
        /// <response code="404">Administrador não encontrado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpDelete("deletarAdmin/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletarAdmin([FromRoute] int id)
        {
            try
            {
                var sucesso = await _adminAppService.DeletarAsync(id);
                if (!sucesso)
                {
                    return NotFound("Admin não encontrado!");
                }

                return Ok("Admin deletado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
