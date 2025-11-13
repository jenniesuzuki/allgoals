using System.ComponentModel.DataAnnotations;

namespace AllGoals.Application.DTOs;

public class UserDtoResponse
{
    /// <example>1</example>
    public int Id { get; set; }

    /// <example>Maria Silva</example>
    [Required]
    public string Nome { get; set; } = string.Empty;

    /// <example>maria@email.com</example>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    /// <example>false</example>
    public bool IsAdmin { get; set; }
    
    public List<LinkDto> Links { get; set; } = new();
}