using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;

namespace AllGoals.Application.Services;

public class GoalService : IGoalService
{
        private readonly IGoalRepository _repo;

        public GoalService(IGoalRepository repo) => _repo = repo;

        public async Task<PagedResultDto<GoalDtoResponse>> ListAsync(PaginationQuery query)
        {
            var (goals, totalCount) = await _repo.GetAllAsync(query.Page, query.PageSize);

            var goalDtos = goals.Select(g =>
            {
                var dto = new GoalDtoResponse
                {
                    Id = g.Id,
                    Titulo = g.Titulo,
                    Descricao = g.Descricao,
                    Xp = g.Xp,
                    Moedas = g.Moedas
                };
                dto.Links.Add(new LinkDto { Rel = "self", Href = $"/api/goals/{dto.Id}", Method = "GET" });
                return dto;
            }).ToList();

            var pagedResult = new PagedResultDto<GoalDtoResponse>
            {
                Items = goalDtos,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };

            AddPaginationLinks(pagedResult, "goals");

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
        public async Task<GoalDtoResponse> CreateAsync(GoalDtoRequest dto)
        {
            var entity = new Goal(
                dto.Titulo,
                dto.Descricao ?? string.Empty, 
                dto.Xp,
                dto.Moedas
            );

            await _repo.AddAsync(entity);
            
            var responseDto = new GoalDtoResponse
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Descricao = entity.Descricao,
                Xp = entity.Xp,
                Moedas = entity.Moedas
            };
            AddGoalLinks(responseDto);

            return responseDto;
        }

        public async Task<bool> UpdateAsync(int id, GoalDtoRequest dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) 
                return false;

            entity.AlterarTitulo(dto.Titulo);
            entity.AlterarDescricao(dto.Descricao ?? string.Empty);
            entity.AlterarRecompensas(dto.Xp, dto.Moedas);

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<GoalDtoResponse?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) 
                return null;

            var responseDto = new GoalDtoResponse
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Descricao = entity.Descricao,
                Xp = entity.Xp,
                Moedas = entity.Moedas
            };

            AddGoalLinks(responseDto);

            return responseDto;
        }

        public async Task<IEnumerable<GoalDtoResponse>> ListAsync()
        {
            var list = await _repo.ListAsync();

            return list.Select(entity =>
            {
                var dto = new GoalDtoResponse
                {
                    Id = entity.Id,
                    Titulo = entity.Titulo,
                    Descricao = entity.Descricao,
                    Xp = entity.Xp,
                    Moedas = entity.Moedas
                };

                dto.Links.Add(new LinkDto
                {
                    Rel = "self",
                    Href = $"/api/goals/{entity.Id}",
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

        private void AddGoalLinks(GoalDtoResponse dto)
        {
            var id = dto.Id;
            
            dto.Links.Add(new LinkDto
            {
                Rel = "self",
                Href = $"/api/goals/{id}",
                Method = "GET"
            });
            dto.Links.Add(new LinkDto
            {
                Rel = "update",
                Href = $"/api/goals/{id}",
                Method = "PUT"
            });
            dto.Links.Add(new LinkDto
            {
                Rel = "delete",
                Href = $"/api/goals/{id}",
                Method = "DELETE"
            });
        }
}