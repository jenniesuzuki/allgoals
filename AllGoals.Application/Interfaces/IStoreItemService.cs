using AllGoals.Application.DTOs;

namespace AllGoals.Application.Interfaces;

public interface IStoreItemService
{
    Task<IEnumerable<StoreItemDtoResponse>> ListAsync();
    Task<StoreItemDtoResponse?> GetByIdAsync(int id);
    Task<StoreItemDtoResponse> CreateAsync(StoreItemDtoRequest createRequestDto);
    Task<bool> UpdateAsync(int id, StoreItemDtoRequest updateRequestDto);
    Task<bool> DeleteAsync(int id);
}