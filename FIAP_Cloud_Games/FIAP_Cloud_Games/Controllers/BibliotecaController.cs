using Core.Entity;
using Core.Input.biblioteca;
using Core.Input.jogo;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAP_Cloud_Games.Controllers
{
    [Controller]
    [Route("v1/[controller]")]
    public class BibliotecaController : Controller
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;

        public BibliotecaController(IBibliotecaRepository bibliotecaRepository)
        {
            _bibliotecaRepository = bibliotecaRepository;
        }


        [HttpGet("obterDadosBiblioteca")] // -> apenas admin
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ObterDadosBiblioteca()
        {
            try
            {
                var lista = await _bibliotecaRepository.ObterDadosBiblioteca();
                if (lista == null || lista.Count == 0)
                {
                    return NotFound("Nenhum dado encontrado!");
                }

                var listaDTO = new List<BibliotecaDTO>();
                foreach (var info in lista)
                {
                    listaDTO.Add(new BibliotecaDTO()
                    {
                        JogoId = info.JogoID,
                        NomeJogo = info.Jogo.NomeJogo,
                        Genero = info.Jogo.Genero,
                        Descricao = info.Jogo.Descricao,
                        Desenvolvedor = info.Jogo.Desenvolvedor,
                        UsuarioId = info.UsuarioID,
                        UserName = info.Usuario.UserName,
                        Email = info.Usuario.Email,
                        JogoEmprestado = info.JogoEmprestado,
                        EstaEmprestado = info.EstaEmprestado,
                        DataAquisicao = info.DataAquisicao,
                    });
                }

                return Ok(listaDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet("obterDadosBibliotecaUsuario/{UsuarioId:int}")] // -> apenas admin
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ObterDadosBibliotecaUsuario([FromRoute] int UsuarioId)
        {
            try
            {
                var dadosUsuario = await _bibliotecaRepository.ObterDadosBibliotecaUsuario(UsuarioId);
                if (dadosUsuario == null || dadosUsuario.Count == 0)
                {
                    return NotFound("Nenhum dado encontrado!");
                }


                var listaDTO = new List<BibliotecaDTO>();
                foreach (var info in dadosUsuario)
                {
                    listaDTO.Add(new BibliotecaDTO()
                    {
                        JogoId = info.JogoID,
                        NomeJogo = info.Jogo.NomeJogo,
                        Genero = info.Jogo.Genero,
                        Descricao = info.Jogo.Descricao,
                        Desenvolvedor = info.Jogo.Desenvolvedor,
                        UsuarioId = info.UsuarioID,
                        UserName = info.Usuario.UserName,
                        Email = info.Usuario.Email,
                        JogoEmprestado = info.JogoEmprestado,
                        EstaEmprestado = info.EstaEmprestado,
                        DataAquisicao = info.DataAquisicao,
                    });
                }

                return Ok(listaDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet("listarJogosPossuidos")] // -> apenas user
        [Authorize(Roles = "User")]
        public async Task<ActionResult> ListarJogosPossuido()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var dadosUsuario = await _bibliotecaRepository.ObterDadosBibliotecaUsuario(userId);
                if (dadosUsuario == null || dadosUsuario.Count == 0)
                {
                    return NotFound("Nenhum dado encontrado!");
                }


                var listaDTO = new List<BibliotecaJogoDTO>();
                foreach (var info in dadosUsuario)
                {
                    listaDTO.Add(new BibliotecaJogoDTO()
                    {
                        JogoId = info.JogoID,
                        NomeJogo = info.Jogo.NomeJogo,
                        Genero = info.Jogo.Genero,
                        Descricao = info.Jogo.Descricao,
                        Desenvolvedor = info.Jogo.Desenvolvedor,
                        JogoEmprestado = info.JogoEmprestado,
                        EstaEmprestado = info.EstaEmprestado,
                        DataAquisicao = info.DataAquisicao,
                    });
                }

                return Ok(listaDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet("listarJogosEmprestados")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult> listarJogosEmprestados()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var jogosEmprestados = await _bibliotecaRepository.RescolherJogosEmprestados(userId);

                if (jogosEmprestados == null || jogosEmprestados.Count == 0)
                {
                    return NotFound("Nenhum Jogo Emprestado!");
                }

                var listaDTO = new List<BibliotecaJogoDTO>();
                foreach (var info in jogosEmprestados)
                {
                    listaDTO.Add(new BibliotecaJogoDTO()
                    {
                        JogoId = info.JogoID,
                        NomeJogo = info.Jogo.NomeJogo,
                        Genero = info.Jogo.Genero,
                        Descricao = info.Jogo.Descricao,
                        Desenvolvedor = info.Jogo.Desenvolvedor,
                        JogoEmprestado = info.JogoEmprestado,
                        EstaEmprestado = info.EstaEmprestado,
                        DataAquisicao = info.DataAquisicao,
                    });
                }

                return Ok(listaDTO);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("AdicionarJogo/{jogoId:int}")] // -> usuario 
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AdicionarJogo([FromRoute] int jogoId )
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
 
                var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId,jogoId);
                if (jogoBiblioteca != null)
                {
                    return Conflict("O usuário já possui o jogo 1");
                }


                var jogoNovo = new Biblioteca()
                {
                    UsuarioID = userId,
                    JogoID = jogoId,
                };
                await _bibliotecaRepository.CadastrarAssync(jogoNovo);

                return Created("Jogo criado com sucesso!", jogoNovo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



    
        [HttpPut("emprestarJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> EmprestarJogo([FromRoute] int jogoId )
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);
                if (jogoBiblioteca == null)
                {
                    return NotFound("Nenhum jogo encontrado para esse usuário!");
                }

                jogoBiblioteca.EmprestarJogo();

                await _bibliotecaRepository.AlterarAsync(jogoBiblioteca);
                return Ok("Jogo emprestado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


       
        [HttpPut("devolverJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DevolverJogo([FromRoute] int jogoId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);
                if (jogoBiblioteca == null)
                {
                    return NotFound("Nenhum jogo encontrado para esse usuário!");
                }
               
                jogoBiblioteca.DevolverJogo();

                await _bibliotecaRepository.AlterarAsync(jogoBiblioteca);
                return Ok("Jogo devolvido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("solicitarJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> SolicitarJogo(int jogoId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var jogolib = await _bibliotecaRepository.RecolherJogoUsuarioId(userId,jogoId);

                if (jogolib == null)
                {
                    return NotFound("Nenhum jogo encontrado para esse usuário!");
                }

                jogolib.SolicitarJogo();

                await _bibliotecaRepository.AlterarAsync(jogolib);

                return Ok("Jogo resgatado com sucesso!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("removerJogo/{jogoId:int}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RemoverJogo([FromRoute] int jogoId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var jogoBiblioteca = await _bibliotecaRepository.RecolherJogoUsuarioId(userId, jogoId);
                if (jogoBiblioteca == null)
                {
                    return NotFound("Nenhum jogo encontrado para esse usuário!");
                }
                await _bibliotecaRepository.DeletarAsync(jogoBiblioteca.ID);
                return Ok("Jogo removido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
