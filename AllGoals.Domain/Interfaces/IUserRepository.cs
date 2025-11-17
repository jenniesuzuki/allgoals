using AllGoals.Domain.Entities;

namespace AllGoals.Domain.Interfaces;

public interface IUserRepository
{
    Task<(IEnumerable<User> Items, int TotalCount)> GetAllAsync(int page, int pageSize);
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> ListAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}