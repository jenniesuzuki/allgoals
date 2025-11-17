using AllGoals.Domain.Entities;

namespace AllGoals.Domain.Interfaces;

public interface IStoreItemRepository
{
    Task<(IEnumerable<StoreItem> Items, int TotalCount)> GetAllAsync(int page, int pageSize);
    Task<StoreItem?> GetByIdAsync(int id);
    Task<List<StoreItem>> ListAsync();
    Task AddAsync(StoreItem storeItem);
    Task UpdateAsync(StoreItem storeItem);
    Task DeleteAsync(int id);
}