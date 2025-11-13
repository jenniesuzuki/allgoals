using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;
using AllGoals.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AllGoals.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _ctx;

    public UserRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<User?> GetByIdAsync(int id) => 
        await _ctx.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<List<User>> ListAsync() => 
        await _ctx.Users.AsNoTracking().ToListAsync();

    public async Task AddAsync(User user) 
    { 
        _ctx.Users.Add(user); 
        await _ctx.SaveChangesAsync(); 
    }

    public async Task UpdateAsync(User user) 
    { 
        _ctx.Users.Update(user); 
        await _ctx.SaveChangesAsync(); 
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _ctx.Users.FindAsync(id);
        if (entity != null) 
        { 
            _ctx.Users.Remove(entity); 
            await _ctx.SaveChangesAsync(); 
        }
    }
}