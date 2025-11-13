using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;
using AllGoals.Domain.ValueObjects;

namespace AllGoals.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<UserDtoResponse> CreateAsync(UserDtoRequest dto)
        {
            var emailVO = new Email(dto.Email);
            var entity = new User(dto.Nome, emailVO);

            if (dto.IsAdmin)
            {
                entity.PromoverParaAdmin();
            }

            await _repo.AddAsync(entity);

            return new UserDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Email = entity.Email.Value,
                IsAdmin = entity.IsAdmin
            };
        }

        public async Task<bool> UpdateAsync(int id, UserDtoRequest dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return false; 

            var newEmailVO = new Email(dto.Email);

            entity.AlterarNome(dto.Nome);
            entity.AlterarEmail(newEmailVO); 

            if (dto.IsAdmin != entity.IsAdmin)
            {
                if (dto.IsAdmin)
                    entity.PromoverParaAdmin();
                else
                    entity.RevogarAdmin();
            }

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<UserDtoResponse?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return null;

            return new UserDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Email = entity.Email.Value,
                IsAdmin = entity.IsAdmin
            };
        }

        public async Task<IEnumerable<UserDtoResponse>> ListAsync()
        {
            var list = await _repo.ListAsync();

            return list.Select(m => new UserDtoResponse
            {
                Id = m.Id,
                Nome = m.Nome,
                Email = m.Email.Value,
                IsAdmin = m.IsAdmin
            }).ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return false; 

            await _repo.DeleteAsync(id); 
            return true;
        }

        public async Task PromoteToAdminAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return; 

            entity.PromoverParaAdmin();
            await _repo.UpdateAsync(entity);
        }

        public async Task RevokeAdminAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return; 

            entity.RevogarAdmin();
            await _repo.UpdateAsync(entity);
        }
}