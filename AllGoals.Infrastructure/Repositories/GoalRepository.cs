using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;
using AllGoals.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AllGoals.Infrastructure.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly ApplicationDbContext _ctx;

    public GoalRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<Goal> Items, int TotalCount)> GetAllAsync(int page, int pageSize)
    {
        var totalCount = await _ctx.Goals.CountAsync();
        var items = await _ctx.Goals
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    public async Task<Goal?> GetByIdAsync(int id) => 
        await _ctx.Goals.FirstOrDefaultAsync(g => g.Id == id);

    public async Task<List<Goal>> ListAsync() => 
        await _ctx.Goals.AsNoTracking().ToListAsync();

    public async Task AddAsync(Goal goal) 
    { 
        _ctx.Goals.Add(goal); 
        await _ctx.SaveChangesAsync(); 
    }
    public async Task UpdateAsync(Goal goal) 
    { 
        _ctx.Goals.Update(goal); 
        await _ctx.SaveChangesAsync(); 
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _ctx.Goals.FindAsync(id);
        if (entity != null) 
        { 
            _ctx.Goals.Remove(entity); 
            await _ctx.SaveChangesAsync(); 
        }
    }
}