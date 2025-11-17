using AllGoals.Domain.Entities;

namespace AllGoals.Domain.Interfaces;

public interface IGoalRepository
{
    Task<(IEnumerable<Goal> Items, int TotalCount)> GetAllAsync(int page, int pageSize);
    Task<Goal?> GetByIdAsync(int id);
    Task<List<Goal>> ListAsync();
    Task AddAsync(Goal goal);
    Task UpdateAsync(Goal goal);
    Task DeleteAsync(int id);
}