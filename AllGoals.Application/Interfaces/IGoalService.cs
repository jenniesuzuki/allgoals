using AllGoals.Application.DTOs;

namespace AllGoals.Application.Interfaces;

public interface IGoalService
{
    Task<GoalDtoResponse?> GetByIdAsync(int id);
    Task<IEnumerable<GoalDtoResponse>> ListAsync();
    Task<GoalDtoResponse> CreateAsync(GoalDtoRequest createRequestDto);
    Task<bool> UpdateAsync(int id, GoalDtoRequest updateRequestDto);
    Task<bool> DeleteAsync(int id);
}