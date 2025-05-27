using Core.Entity;
using Core.Input.admin;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FIAP_Cloud_Games.Controllers
{

    [Controller]
    [Route("v1/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
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
                var lista =  await _adminRepository.ObterTodosAsync();

                if (lista == null || lista.Count == 0)
                {
                    return NotFound("Nenhum admin encontrado!");
                }

                var listaDTO = new List<AdminDTO>(); 

                foreach(var admin in lista)
                {
                    listaDTO.Add(new AdminDTO()
                    { 
                        Email = admin.Email,
                        UserName = admin.UserName,
                        Id = admin.ID,
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
                var admin = await _adminRepository.ObterPorIdAsync(id);
                if (admin == null)
                {
                    return NotFound("Admin não encontrado!");
                }
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
                Admin admin = new Admin(adminInput.UserName, adminInput.Senha, adminInput.Email);

                var usuariosAdmins = await _adminRepository
                    .ListarAdminsPorEmailouUserName(adminInput.UserName, adminInput.Email);

                if (usuariosAdmins.Any())
                {
                    return Conflict("Username ou Email já existe");
                }


                AdminDTO adminTDO = new AdminDTO(){
                    UserName = admin.UserName,
                    Email = admin.Email,
                    Id = admin.ID
                };

                await _adminRepository.CadastrarAssync(admin);

                return Created("Admin criado com sucesso !",adminTDO);

            }
            catch(Exception ex)
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
                var admin = await _adminRepository.ObterPorIdAsync(adminUpdateInput.AdminID);
                if (admin == null)
                {
                    return NotFound("Admin não encontrado!");
                }

                admin.UserName = adminUpdateInput.UserName;
                admin.Email = adminUpdateInput.Email;
                admin.Senha = adminUpdateInput.Senha;
                await _adminRepository.AlterarAsync(admin);
                
                AdminDTO adminDTO = new AdminDTO()
                {
                    UserName = admin.UserName,
                    Email = admin.Email,
                    Id = admin.ID
                };

                return Ok(new
                {
                    Messagem = "Admin atualizado com sucesso !",
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
                var admin = await _adminRepository.ObterPorIdAsync(id);
                if (admin == null)
                {
                    return NotFound("Admin não encontrado!");
                }
                await _adminRepository.DeletarAsync(admin.ID);
                return Ok("Admin deletado com sucesso !");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
