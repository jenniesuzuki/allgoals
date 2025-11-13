using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllGoals.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDtoResponse>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.ListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtém um usuário específico pelo Id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserDtoResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserDtoResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateUser([FromBody] UserDtoRequest createDto)
        {
            var newUserResponse = await _userService.CreateAsync(createDto);

            return CreatedAtAction(
                nameof(GetUserById), 
                new { id = newUserResponse.Id }, 
                newUserResponse
            );
        }

        /// <summary>
        /// Atualiza um usuário.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDtoRequest updateDto)
        {
            var success = await _userService.UpdateAsync(id, updateDto);

            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui um usuário.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
}