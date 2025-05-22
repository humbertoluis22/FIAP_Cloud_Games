using Core.Entity;
using Core.Input.admin;
using Core.Repository;
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


        [HttpGet("listarAdmin")]
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


        [HttpGet("recolherAdminid/{id:int}")]
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

        [HttpPost("criarAdmin")]
        public async Task<ActionResult> CriarAdmin([FromBody] AdminInput adminInput )
        {
            try
            {
                Admin admin = new Admin(adminInput.UserName, adminInput.Senha, adminInput.Email);
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



        [HttpPut("atualizarAdmin")]
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


        [HttpDelete("deletarAdmin/{id:int}")]
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



        //[HttpPost("LoginAdmin")]
        //public async Task<ActionResult> RealizarLogin([FromBody] AdminInput adminInput)
        //{
        //    // implemnetar rota depois 
        //}


        //[HttpPut("DesbloquearUsuario/{id:int}")]
        //public async Task<ActionResult> DesbloquearUsuario([FromRoute] int id)
        //{
               // implementar rotas depois -> MOVER PARA ROTA DE USUARIO QUANDO CRIADA
        //}
    }
}
