using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Core.Input.jogo;
using Core.Entity;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class JogoController : Controller
    {
        private readonly IJogoRepository _jogoRepository;
        public JogoController(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }


        [HttpGet("listarJogo")]
        public async Task<ActionResult> ListarJogo()
        {
            try
            {
                var lista = await _jogoRepository.ObterTodosAsync();
                if (lista == null || lista.Count == 0)
                {
                    return NotFound("Nenhum jogo encontrado!");
                }

                var listaDTO = new List<JogoDto>();
                foreach (var jogo in lista)
                {
                    listaDTO.Add(
                        new JogoDto()
                        {
                            JogoId = jogo.ID,
                            NomeJogo = jogo.NomeJogo,
                            Genero = jogo.Genero,
                            Descricao = jogo.Descricao,
                            Desenvolvedor = jogo.Desenvolvedor,
                            Preco = jogo.Preco,

                        });
                }
                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("recolherJogoId/{id:int}")]
        public async Task<ActionResult> RecolherJogoId(int id)
        {
            try
            {
                var jogo = await _jogoRepository.ObterPorIdAsync(id);
                if (jogo == null)
                {
                    return NotFound("Nenhum jogo encontrado!");
                }
                var jogoDTO = new JogoDto()
                {
                    JogoId = jogo.ID,
                    NomeJogo = jogo.NomeJogo,
                    Genero = jogo.Genero,
                    Descricao = jogo.Descricao,
                    Desenvolvedor = jogo.Desenvolvedor,
                    Preco = jogo.Preco,
                };
                return Ok(jogoDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("criarJogo")]
        public async Task<ActionResult> CriarJogo([FromBody] JogoInput jogoInput)
        {
            try
            {
                Jogo jogo = new Jogo(
                    jogoInput.IdAdmin,
                    jogoInput.NomeJogo,
                    jogoInput.Genero,
                    jogoInput.Descricao,
                    jogoInput.Desenvolvedor,
                    jogoInput.Preco);

                await _jogoRepository.CadastrarAssync(jogo);

                var jogoDTO = new JogoDto()
                {
                    JogoId = jogo.ID,
                    NomeJogo = jogo.NomeJogo,
                    Genero = jogo.Genero,
                    Descricao = jogo.Descricao,
                    Desenvolvedor = jogo.Desenvolvedor,
                    Preco = jogo.Preco,
                };

                return Created("Jogo criado com sucesso !", jogoDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("atualizarJogo")]
        public async Task<ActionResult> AtualizarJogo([FromBody] JogoUpdateInput jogoInput)
        {
            try
            {

                var jogo = await _jogoRepository.ObterPorIdAsync(jogoInput.JogoID);
                
                if (jogo == null)
                {
                    return NotFound("Jogo não encontrado!");
                }   

                jogo.NomeJogo = jogoInput.NomeJogo;
                jogo.Genero = jogoInput.Genero;
                jogo.Descricao = jogoInput.Descricao;
                jogo.Preco = jogoInput.Preco;

                await _jogoRepository.AlterarAsync(jogo);

                var jogoDTO = new JogoDto()
                {
                    JogoId = jogo.ID,
                    NomeJogo = jogo.NomeJogo,
                    Genero = jogo.Genero,
                    Descricao = jogo.Descricao,
                    Desenvolvedor = jogo.Desenvolvedor,
                    Preco = jogo.Preco,
                };

                return Ok(new
                {
                    Messagem = "Jogo atualizado com sucesso !",
                    Dados = jogoDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("deletarJogo/{id:int}")]
        public async Task<ActionResult> DeletarJogo(int id)
        {
            try
            {
                var jogo = await _jogoRepository.ObterPorIdAsync(id);
                if (jogo == null)
                {
                    return NotFound("Jogo não encontrado!");
                }
                await _jogoRepository.DeletarAsync(jogo.ID);
                return Ok("Jogo deletado com sucesso !");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
  
    
    
    }
}
