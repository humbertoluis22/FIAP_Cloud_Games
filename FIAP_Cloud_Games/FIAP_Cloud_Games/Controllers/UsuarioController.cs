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



        // essa rota é de usuario apos autenticação nao pedir o usuario id no input 
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

        //[HttpPut("BloquearUsuario")] -> apenas admin
        //[HttpPut("DesbloquearUsuario")] -> apenas admin


    }
}
