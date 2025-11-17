using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AllGoals.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GoalController : ControllerBase
{
    private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

//        /// <summary>
//        /// Obtém as metas com paginação.
//        /// </summary>
//        [HttpGet]
//        [ProducesResponseType(typeof(IEnumerable<GoalDtoResponse>), 200)]
//        public async Task<IActionResult> GetGoals()
//        {
//            var goals = await _goalService.ListAsync();
//            return Ok(goals);
//        }

        /// <summary>
        /// Obtém todas as metas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<GoalDtoResponse>), 200)]
        public async Task<IActionResult> GetGoals([FromQuery] PaginationQuery query)
        {
            if (query.Page < 1 || query.PageSize < 1) 
                return BadRequest("Parâmetros de paginação inválidos.");

            var result = await _goalService.ListAsync(query);
            return Ok(result);
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