using InfraEstructure;
using InfraEstructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenGenerate _tokenGenerate;
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context,TokenGenerate tokenGenerate)
        {
            _context = context;
            _tokenGenerate = tokenGenerate;

        }

        [HttpPost("autenticar")]
        public ActionResult Autenticar(string userName, string role)
        {
            //var usuario =  _context.Usuarios.FirstOrDefault(u => u.UserName == userName);
            var token = _tokenGenerate.GenerateToken(userName, role);
            if(userName == "Humberto")
            {
                return BadRequest("usuario invalido");
            }
            return Ok(new { token = token});
        }
    }
}
