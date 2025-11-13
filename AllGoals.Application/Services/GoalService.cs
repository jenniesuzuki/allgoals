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
                dto.Descricao,
                dto.Xp,
                dto.Moedas
            );

            await _repo.AddAsync(entity);
            
            return new GoalDtoResponse
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Descricao = entity.Descricao,
                Xp = entity.Xp,
                Moedas = entity.Moedas
            };
        }

        public async Task<bool> UpdateAsync(int id, GoalDtoRequest dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) 
                return false;

            entity.AlterarTitulo(dto.Titulo);
            entity.AlterarDescricao(dto.Descricao);
            entity.AlterarRecompensas(dto.Xp, dto.Moedas);

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<GoalDtoResponse?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) 
                return null;

            return new GoalDtoResponse
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Descricao = entity.Descricao,
                Xp = entity.Xp,
                Moedas = entity.Moedas
            };
        }

        public async Task<IEnumerable<GoalDtoResponse>> ListAsync()
        {
            var list = await _repo.ListAsync();

            return list.Select(m => new GoalDtoResponse
            {
                Id = m.Id,
                Titulo = m.Titulo,
                Descricao = m.Descricao,
                Xp = m.Xp,
                Moedas = m.Moedas
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
}