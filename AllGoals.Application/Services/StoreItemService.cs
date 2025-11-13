using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;

namespace AllGoals.Application.Services;

public class StoreItemService : IStoreItemService
{
        private readonly IStoreItemRepository _repo;

        public StoreItemService(IStoreItemRepository repo) => _repo = repo;

        public async Task<StoreItemDtoResponse> CreateAsync(StoreItemDtoRequest dto)
        {
            var entity = new StoreItem(
                dto.Nome,
                dto.Descricao,
                dto.Valor
            );

            await _repo.AddAsync(entity);

            return new StoreItemDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Descricao = entity.Descricao,
                Valor = entity.Valor
            };
        }

        public async Task<bool> UpdateAsync(int id, StoreItemDtoRequest dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return false;

            entity.AlterarNome(dto.Nome);
            entity.AlterarDescricao(dto.Descricao);
            entity.AlterarValor(dto.Valor);

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<StoreItemDtoResponse?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return null;

            return new StoreItemDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Descricao = entity.Descricao,
                Valor = entity.Valor
            };
        }

        public async Task<IEnumerable<StoreItemDtoResponse>> ListAsync()
        {
            var list = await _repo.ListAsync();

            return list.Select(m => new StoreItemDtoResponse
            {
                Id = m.Id,
                Nome = m.Nome,
                Descricao = m.Descricao,
                Valor = m.Valor
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