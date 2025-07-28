using Microsoft.AspNetCore.Mvc;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class HealthController : Controller
    {

        /// <summary>
        /// validar saude da aplicacao.
        /// </summary>
        /// <remarks>
        /// Requisição pública para validar a saude da aplicacao.
        /// Não exige autenticação.
        /// </remarks>
        /// <returns>Retorna uma mensagem simples.</returns>
        /// <response code="200">Retorna uma mensagem simples.</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet]
        public async Task<ActionResult> CheckHealth()
        {
            try
            {
                return Ok(new { status = "Healthy" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
