using Core.Entity;
using Core.Input.usuario;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
                var lista = await _usuarioRepository.ObterTodosAsync();
                if (lista == null || lista.Count == 0)
                {
                    return NotFound("Nenhum usuario encontrado !");
                }
                var listaDTO = new List<UsuarioDTO>();
                foreach (var usuario in lista)
                {
                    listaDTO.Add(new UsuarioDTO()
                    {
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Id = usuario.ID,
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
                var usuariosBloqueados = await _usuarioRepository.RecolherUsuarioBloqueados();
                if (!usuariosBloqueados.Any())
                {
                    return NotFound("Nenhum usuario bloqueado encontrado !");
                }
                var usuariosBloqueadosDTO = new List<UsuarioBloqueadoDTO>();
                foreach (var usuario in usuariosBloqueados)
                {
                    usuariosBloqueadosDTO.Add(new UsuarioBloqueadoDTO()
                    {
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Id = usuario.ID,
                        Bloqueado = usuario.Bloqueado

                    });
                }

                return Ok(usuariosBloqueadosDTO);
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
                Usuario usuario = new Usuario(
                    usuarioInput.UserName,
                    usuarioInput.Senha,
                    usuarioInput.Email
                    );


                var usuario_validacao = await _usuarioRepository
                    .ListarUsuarioPorEmailouUserName(usuarioInput.UserName, usuarioInput.Email);


                if (usuario_validacao.Any())
                {
                    return Conflict("UserName ou Email já utilizados!");
                }

                await _usuarioRepository.CadastrarAssync(usuario);

                UsuarioDTO usuarioTDO = new UsuarioDTO()
                {
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    Id = usuario.ID,
                };

                return Created("Usuario criado com sucesso !", usuarioTDO);
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
                var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioInput.UsuarioID);
                if (usuario == null)
                {
                    return NotFound("Usuario não encontrado");
                }

                usuario.AlterarSenha(usuarioInput.Senha);
                await _usuarioRepository.AlterarAsync(usuario);

                UsuarioDTO usuarioTDO = new UsuarioDTO()
                {
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    Id = usuario.ID,
                };
                return Ok(new
                {
                    Messagem = "Senha alterada com sucesso !",
                    Dados = usuarioTDO
                });
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
                var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioInput.UsuarioID);
                if (usuario == null)
                {
                    return NotFound("Nenhum Usuário encontrado !");
                }
                usuario.Email = usuarioInput.Email;
                await _usuarioRepository.AlterarAsync(usuario);

                var usuarioDto = new UsuarioDTO()
                {
                    Email = usuario.Email,
                    UserName = usuario.UserName,
                    Id = usuario.ID,
                };

                return Ok(new
                {
                    messagem = "Email alterado com sucesso !",
                    dados = usuarioDto
                });
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
                var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
                if (usuario == null)
                {
                    return NotFound("Nenhum usuário encontrado !");
                }
                usuario.BloquearUsuario();
                await _usuarioRepository.AlterarAsync(usuario);
                return Ok("Usuario bloqueado com sucesso !");
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
                var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
                if (usuario == null)
                {
                    return NotFound("Nenhum usuário encontrado !");
                }
                usuario.DesbloquearUsuario();
                await _usuarioRepository.AlterarAsync(usuario);
                return Ok("Usuario desbloqueado com sucesso !");
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
                var usuario = _usuarioRepository.ObterPorIdAsync(usuarioId);
                if (usuario == null)
                {
                    return NotFound("Nenhum usuário encontrado !");
                }
                await _usuarioRepository.DeletarAsync(usuarioId);
                return Ok("Usuario deletado com sucesso !");

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

                var usuario = await _usuarioRepository.ObterPorIdAsync(userId);
                if (usuario == null)
                {
                    return NotFound("Nenhum usuário encontrado !");
                }
                await _usuarioRepository.DeletarAsync(usuario.ID);
                return Ok("Conta deletada com sucesso !");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

    }
}
