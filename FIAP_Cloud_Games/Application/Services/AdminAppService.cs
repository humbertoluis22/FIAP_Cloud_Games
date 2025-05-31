using Core.Entity;
using Core.Input;
using Core.Input.admin;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public  class AdminAppService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminAppService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<AdminDTO>> ListarTodosAsync()
        {
            var admins = await _adminRepository.ObterTodosAsync();

            return admins.Select(admin => new AdminDTO
            {
                Email = admin.Email,
                UserName = admin.UserName,
                Id = admin.ID
            }).ToList();
        }

        public async Task<AdminDTO?> ObterPorIdAsync(int id)
        {
            var admin = await _adminRepository.ObterPorIdAsync(id);
            if (admin == null) return null;

            return new AdminDTO
            {
                Email = admin.Email,
                UserName = admin.UserName,
                Id = admin.ID
            };
        }

        public async Task<AdminDTO?> CriarAsync(AdminInput adminInput)
        {
            var existente = await _adminRepository
                .ListarAdminsPorEmailouUserName(adminInput.UserName, adminInput.Email);

            if (existente.Any())
                throw new InvalidOperationException("Username ou Email já existe");

            var admin = new Admin(adminInput.UserName, adminInput.Senha, adminInput.Email);
            await _adminRepository.CadastrarAssync(admin);

            return new AdminDTO
            {
                Email = admin.Email,
                UserName = admin.UserName,
                Id = admin.ID
            };
        }

        public async Task<AdminDTO?> AtualizarAsync(AdminUpdateInput adminUpdateInput)
        {
            var admin = await _adminRepository.ObterPorIdAsync(adminUpdateInput.AdminID);
            if (admin == null) return null;

            admin.UserName = adminUpdateInput.UserName;
            admin.Email = adminUpdateInput.Email;
            admin.Senha = adminUpdateInput.Senha;

            await _adminRepository.AlterarAsync(admin);

            return new AdminDTO
            {
                Email = admin.Email,
                UserName = admin.UserName,
                Id = admin.ID
            };
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var admin = await _adminRepository.ObterPorIdAsync(id);
            if (admin == null)
            {
                return false;
            }
            await _adminRepository.DeletarAsync(id);
            return true;
        }
    }
}
