using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;

namespace AllGoals.Application.Services;

public class GoalService : IGoalService
{
        private readonly IGoalRepository _repo;

        public GoalService(IGoalRepository repo) => _repo = repo;

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