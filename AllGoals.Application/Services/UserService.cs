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

            await _repo.AddAsync(entity);
            
            var responseDto = new UserDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Email = entity.Email.Value,
                IsAdmin = entity.IsAdmin
            };
            
            AddUserLinks(responseDto);

            return responseDto;
        }

        public async Task<bool> UpdateAsync(int id, UserDtoRequest dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return false; 

            var newEmailVO = new Email(dto.Email);

            entity.AlterarNome(dto.Nome);
            entity.AlterarEmail(newEmailVO); 

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<UserDtoResponse?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return null;

            var responseDto = new UserDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Email = entity.Email.Value,
                IsAdmin = entity.IsAdmin
            };
            
            AddUserLinks(responseDto);

            return responseDto;
        }

        public async Task<IEnumerable<UserDtoResponse>> ListAsync()
        {
            var list = await _repo.ListAsync();

            return list.Select(entity =>
            {
                var dto = new UserDtoResponse
                {
                    Id = entity.Id,
                    Nome = entity.Nome,
                    Email = entity.Email.Value,
                    IsAdmin = entity.IsAdmin
                };
                
                dto.Links.Add(new LinkDto
                {
                    Rel = "self",
                    Href = $"/api/users/{dto.Id}",
                    Method = "GET"
                });

                return dto;
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

        private void AddUserLinks(UserDtoResponse dto)
        {
            var id = dto.Id;
            
            dto.Links.Add(new LinkDto
            {
                Rel = "self",
                Href = $"/api/users/{id}",
                Method = "GET"
            });
            dto.Links.Add(new LinkDto
            {
                Rel = "update",
                Href = $"/api/users/{id}",
                Method = "PUT"
            });
            dto.Links.Add(new LinkDto
            {
                Rel = "delete",
                Href = $"/api/users/{id}",
                Method = "DELETE"
            });
        }
}