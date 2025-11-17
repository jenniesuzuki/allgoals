using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AllGoals.API.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/user")]
public class UserControllerV2 : ControllerBase
{
    private readonly IUserService _userService;

    public UserControllerV2(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<UserDtoResponse>), 200)]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationQuery query)
    {
        var result = await _userService.ListAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserDtoResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDtoResponse), 201)]
    public async Task<IActionResult> CreateUser([FromBody] UserDtoRequest createDto)
    {
        var newUser = await _userService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id, version = "2.0" }, newUser);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDtoRequest updateDto)
    {
        var success = await _userService.UpdateAsync(id, updateDto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Promove um usuário a Administrador (Apenas V2).
    /// </summary>
    [HttpPost("{id:int}/promote")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PromoteUser(int id)
    {
        await _userService.PromoteToAdminAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Revoga privilégios de Administrador (Apenas V2).
    /// </summary>
    [HttpPost("{id:int}/revoke")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RevokeUser(int id)
    {
        await _userService.RevokeAdminAsync(id);
        return NoContent();
    }
}