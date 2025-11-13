using AllGoals.Application.DTOs;

namespace AllGoals.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDtoResponse>> ListAsync();
    Task<UserDtoResponse?> GetByIdAsync(int id);
    Task<UserDtoResponse> CreateAsync(UserDtoRequest createRequestDto);
    Task<bool> UpdateAsync(int id, UserDtoRequest updateRequestDto);
    Task<bool> DeleteAsync(int id);
    Task PromoteToAdminAsync(int id);
    Task RevokeAdminAsync(int id);
}