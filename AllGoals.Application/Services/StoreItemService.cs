using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;

namespace AllGoals.Application.Services;

public class StoreItemService : IStoreItemService
{
        private readonly IStoreItemRepository _repo;

        public StoreItemService(IStoreItemRepository repo) => _repo = repo;
        
        public async Task<PagedResultDto<StoreItemDtoResponse>> ListAsync(PaginationQuery query)
        {
            var (items, totalCount) = await _repo.GetAllAsync(query.Page, query.PageSize);

            var itemDtos = items.Select(i =>
            {
                var dto = new StoreItemDtoResponse
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Descricao = i.Descricao,
                    Valor = i.Valor
                };

                dto.Links.Add(new LinkDto { Rel = "self", Href = $"/api/storeitems/{dto.Id}", Method = "GET" });
                return dto;
            }).ToList();

            var pagedResult = new PagedResultDto<StoreItemDtoResponse>
            {
                Items = itemDtos,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };

            AddPaginationLinks(pagedResult, "storeitems");

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

        public async Task<StoreItemDtoResponse> CreateAsync(StoreItemDtoRequest dto)
        {
            var entity = new StoreItem(
                dto.Nome,
                dto.Descricao ?? string.Empty, 
                dto.Valor
            );

            await _repo.AddAsync(entity);

            var responseDto = new StoreItemDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Descricao = entity.Descricao,
                Valor = entity.Valor
            };

            AddStoreItemLinks(responseDto);

            return responseDto;
        }

        public async Task<bool> UpdateAsync(int id, StoreItemDtoRequest dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return false;

            entity.AlterarNome(dto.Nome);
            entity.AlterarDescricao(dto.Descricao ?? string.Empty);
            entity.AlterarValor(dto.Valor);

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<StoreItemDtoResponse?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null)
                return null;

            var responseDto = new StoreItemDtoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Descricao = entity.Descricao,
                Valor = entity.Valor
            };

            AddStoreItemLinks(responseDto);

            return responseDto;
        }

        public async Task<IEnumerable<StoreItemDtoResponse>> ListAsync()
        {
            var list = await _repo.ListAsync();
            return list.Select(entity =>
            {
                var dto = new StoreItemDtoResponse
                {
                    Id = entity.Id,
                    Nome = entity.Nome,
                    Descricao = entity.Descricao,
                    Valor = entity.Valor
                };
                
                dto.Links.Add(new LinkDto
                {
                    Rel = "self",
                    Href = $"/api/storeitems/{entity.Id}",
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

        private void AddStoreItemLinks(StoreItemDtoResponse dto)
        {
            var id = dto.Id;
            
            dto.Links.Add(new LinkDto
            {
                Rel = "self",
                Href = $"/api/storeitems/{id}",
                Method = "GET"
            });
            dto.Links.Add(new LinkDto
            {
                Rel = "update",
                Href = $"/api/storeitems/{id}",
                Method = "PUT"
            });
            dto.Links.Add(new LinkDto
            {
                Rel = "delete",
                Href = $"/api/storeitems/{id}",
                Method = "DELETE"
            });
        }
}