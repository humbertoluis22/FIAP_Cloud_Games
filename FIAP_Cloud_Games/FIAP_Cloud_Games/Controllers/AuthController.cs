using Core.Input.admin;
using Core.Input.usuario;
using Core.Repository;
using InfraEstructure;
using InfraEstructure.Auth;
using InfraEstructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenGenerate _tokenGenerate;
        private readonly IAdminRepository _adminRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthController(
            IAdminRepository adminRepository,
            IUsuarioRepository  usuarioRepository, 
            TokenGenerate tokenGenerate
            )
        {
            _adminRepository = adminRepository;
            _usuarioRepository = usuarioRepository;
            _tokenGenerate = tokenGenerate;

        }


        [HttpPost("loginUsuario")] //-> usuario
        public async Task<ActionResult> RealizarLoginUsuario([FromBody] UsuarioInput usuarioInput)
        {
            try
            {
                var usuario_validacao = await _usuarioRepository
                    .recolherusuarioPorEmailUsername(usuarioInput.UserName,usuarioInput.Email);


                if (usuario_validacao == null) 
                {
                    return NotFound("UserName ou Email invalidos!");
                }
                else if (usuario_validacao.Bloqueado)
                {
                    return NotFound("Usuario Bloqueado,entre em contato com o Admin");
                }

                var usuario = await _usuarioRepository
                    .ObterPorLogin(usuarioInput.UserName, usuarioInput.Email, usuarioInput.Senha);


                if (usuario == null)
                {
                    usuario_validacao.AdicionarTentativaErrada();
                    await _usuarioRepository.AlterarAsync(usuario_validacao);
                    return NotFound("Senha Incorreta");
                }

                usuario.ZerarTentativasErrada();
                await _usuarioRepository.AlterarAsync(usuario);

                var token = _tokenGenerate.GenerateToken(usuario.ID,usuario.UserName, "User");
                return Ok(new
                {
                    Messagem = "Token gerado com sucesso",
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("LoginAdmin")]
        public async Task<ActionResult> RealizarLoginAdmin([FromBody] AdminInput adminInput)
        {
            try
            {
                var admin = await _adminRepository
                    .ObterPorLogin(adminInput.UserName,adminInput.Email ,adminInput.Senha);
                
                if (admin == null)
                {
                    return NotFound("Usuario ou senha invalidos");
                }

                var token = _tokenGenerate.GenerateToken(admin.ID,admin.UserName, "Admin");
                return Ok(new 
                {
                    Messagem = "Token gerado com sucesso",
                    Token = token 
                });

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
