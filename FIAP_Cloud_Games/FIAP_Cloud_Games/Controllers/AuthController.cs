using Core.Input;
using Core.Repository;
using InfraEstructure.Auth;
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



        /// <summary>
        /// Realiza o login de um usuário comum.
        /// </summary>
        /// <remarks>
        /// Gera um token de autenticação caso as credenciais sejam válidas.
        /// </remarks>
        /// <param name="usuarioInput">Dados de autenticação do usuário (username, email e senha).</param>
        /// <returns>Token de autenticação para o usuário.</returns>
        /// <response code="200">Token gerado com sucesso</response>
        /// <response code="404">Usuário não encontrado ou credenciais inválidas</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPost("loginUsuario")] 
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



        /// <summary>
        /// Realiza o login de um administrador.
        /// </summary>
        /// <remarks>
        /// Gera um token de autenticação para administradores.
        /// </remarks>
        /// <param name="adminInput">Dados de autenticação do administrador (username, email e senha).</param>
        /// <returns>Token de autenticação para o administrador.</returns>
        /// <response code="200">Token gerado com sucesso</response>
        /// <response code="404">Administrador não encontrado ou credenciais inválidas</response>
        /// <response code="400">Erro na requisição</response>
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
