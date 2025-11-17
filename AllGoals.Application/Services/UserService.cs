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

        public async Task<PagedResultDto<UserDtoResponse>> ListAsync(PaginationQuery query)
        {
            var (users, totalCount) = await _repo.GetAllAsync(query.Page, query.PageSize);

            var userDtos = users.Select(u => 
            {
                var dto = new UserDtoResponse
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email.Value,
                    IsAdmin = u.IsAdmin
                };

                dto.Links.Add(new LinkDto { Rel = "self", Href = $"/api/users/{u.Id}", Method = "GET" });
                return dto;
            }).ToList();

            var pagedResult = new PagedResultDto<UserDtoResponse>
            {
                Items = userDtos,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };

            AddPaginationLinks(pagedResult, "users");

            return pagedResult;
        }

        private void AddPaginationLinks<T>(PagedResultDto<T> result, string route)
        {
            result.Links.Add(new LinkDto 
            { 
                Rel = "self", 
                Href = $"/api/{route}?page={result.Page}&pageSize={result.PageSize}", 
                Method = "GET" 
            });

            if (result.Page < result.TotalPages) 
            {
                result.Links.Add(new LinkDto 
                { 
                    Rel = "next", 
                    Href = $"/api/{route}?page={result.Page + 1}&pageSize={result.PageSize}", 
                    Method = "GET" 
                });
            }

            if (result.Page > 1) 
            {
                result.Links.Add(new LinkDto 
                { 
                    Rel = "previous", 
                    Href = $"/api/{route}?page={result.Page - 1}&pageSize={result.PageSize}", 
                    Method = "GET" 
                });
            }
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