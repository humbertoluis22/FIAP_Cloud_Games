using Core.Entity;
using Core.Input.usuario;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost("cadastrarUsuario")]
        public async Task<ActionResult> CadastrarUsuario([FromBody] UsuarioInput usuarioInput)
        {
            try
            {
                Usuario usuario = new Usuario(
                    usuarioInput.UserName,
                    usuarioInput.Senha, 
                    usuarioInput.Email
                    );

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
        public async Task<ActionResult> AlterarSenha([FromBody] UsuarioUpdateInput usuarioInput )
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioInput.UsuarioID);
                if(usuario == null )
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
                return Ok(new {
                    Messagem = "Senha alterada com sucesso !",
                    Dados = usuarioTDO
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
