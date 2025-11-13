using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllGoals.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalController : ControllerBase
{
    private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        /// <summary>
        /// Obtém todas as metas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GoalDtoResponse>), 200)]
        public async Task<IActionResult> GetGoals()
        {
            var goals = await _goalService.ListAsync();
            return Ok(goals);
        }

        /// <summary>
        /// Obtém uma meta específica pelo Id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GoalDtoResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetGoalById(int id)
        {
            var goal = await _goalService.GetByIdAsync(id);

            if (goal == null)
                return NotFound();

            return Ok(goal);
        }

        /// <summary>
        /// Cria uma nova meta.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GoalDtoResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGoal([FromBody] GoalDtoRequest createDto)
        {
            var newGoalResponse = await _goalService.CreateAsync(createDto);

            return CreatedAtAction(
                nameof(GetGoalById),
                new { id = newGoalResponse.Id },
                newGoalResponse
            );
        }

        /// <summary>
        /// Atualiza uma meta.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] GoalDtoRequest updateDto)
        {
            var success = await _goalService.UpdateAsync(id, updateDto);

            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui uma meta.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var success = await _goalService.DeleteAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
}