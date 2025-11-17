using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;
using AllGoals.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AllGoals.Infrastructure.Repositories;

public class StoreItemRepository : IStoreItemRepository
{
    private readonly ApplicationDbContext _ctx;

    public StoreItemRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<StoreItem> Items, int TotalCount)> GetAllAsync(int page, int pageSize)
    {
        var totalCount = await _ctx.StoreItems.CountAsync(); 
        var items = await _ctx.StoreItems
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    public async Task<StoreItem?> GetByIdAsync(int id) => 
        await _ctx.StoreItems.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<List<StoreItem>> ListAsync() => 
        await _ctx.StoreItems.AsNoTracking().ToListAsync(); 

    public async Task AddAsync(StoreItem item) 
    { 
        _ctx.StoreItems.Add(item);
        await _ctx.SaveChangesAsync(); 
    }

    public async Task UpdateAsync(StoreItem item) 
    { 
        _ctx.StoreItems.Update(item);
        await _ctx.SaveChangesAsync(); 
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _ctx.StoreItems.FindAsync(id);
        if (entity != null) 
        { 
            _ctx.StoreItems.Remove(entity); 
            await _ctx.SaveChangesAsync(); 
        }
    }
}