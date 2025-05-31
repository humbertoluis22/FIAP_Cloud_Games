using Core.Entity;
using Core.Input;
using Core.Input.usuario;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserAppService
    {
        private readonly IUsuarioRepository _userRepository;

        public UserAppService(IUsuarioRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<List<UsuarioDTO>> ListarTodosAsync()
        {
            var users = await _userRepository.ObterTodosAsync();
            return users.Select(user => new UsuarioDTO
            {
                Email = user.Email,
                UserName = user.UserName,
                Id = user.ID
            }).ToList();
        }

        public async Task<List<UsuarioDTO>> ListarBloqueadosAsync()
        {
            var users = await _userRepository.RecolherUsuarioBloqueados();
            return users.Select(user => new UsuarioDTO
            {
                Email = user.Email,
                UserName = user.UserName,
                Id = user.ID
            }).ToList();
        }

        public async Task<UsuarioDTO> CadastrarUsuarioAsync(UsuarioInput input)
        {
            var jaExiste = await _userRepository
                .ListarUsuarioPorEmailouUserName(input.UserName, input.Email);

            if (jaExiste.Any())
                throw new InvalidOperationException("UserName ou Email já utilizados!");

            var usuario = new Usuario(input.UserName, input.Senha, input.Email);
            await _userRepository.CadastrarAssync(usuario);

            return new UsuarioDTO
            {
                UserName = usuario.UserName,
                Email = usuario.Email,
                Id = usuario.ID
            };
        }

        public async Task<UsuarioDTO> AlterarSenhaAsync(UsuarioUpdateInput input)
        {
            var usuario = await _userRepository.ObterPorIdAsync(input.UsuarioID)
                          ?? throw new KeyNotFoundException("Usuario não encontrado");

            usuario.AlterarSenha(input.Senha);
            await _userRepository.AlterarAsync(usuario);

            return new UsuarioDTO
            {
                Email = usuario.Email,
                UserName = usuario.UserName,
                Id = usuario.ID
            };
        }

        public async Task<UsuarioDTO> AlterarEmailAsync(UsuarioUpdateEmailInput input)
        {
            var usuario = await _userRepository.ObterPorIdAsync(input.UsuarioID)
                          ?? throw new KeyNotFoundException("Usuario não encontrado");

            usuario.Email = input.Email;
            await _userRepository.AlterarAsync(usuario);

            return new UsuarioDTO
            {
                Email = usuario.Email,
                UserName = usuario.UserName,
                Id = usuario.ID
            };
        }

        public async Task BloquearUsuarioAsync(int usuarioId)
        {
            var usuario = await _userRepository.ObterPorIdAsync(usuarioId)
                          ?? throw new KeyNotFoundException("Usuario não encontrado");

            usuario.BloquearUsuario();
            await _userRepository.AlterarAsync(usuario);
        }

        public async Task DesbloquearUsuarioAsync(int usuarioId)
        {
            var usuario = await _userRepository.ObterPorIdAsync(usuarioId)
                          ?? throw new KeyNotFoundException("Usuario não encontrado");

            usuario.DesbloquearUsuario();
            await _userRepository.AlterarAsync(usuario);
        }

        public async Task DeletarUsuarioAsync(int usuarioId)
        {
            var usuario = await _userRepository.ObterPorIdAsync(usuarioId)
                          ?? throw new KeyNotFoundException("Usuario não encontrado");

            await _userRepository.DeletarAsync(usuarioId);
        }

    }
}
