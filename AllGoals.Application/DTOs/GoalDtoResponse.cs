using System.ComponentModel.DataAnnotations;

namespace AllGoals.Application.DTOs;

public class GoalDtoResponse
{
    /// <example>1</example>
    public int Id { get; set; }

    /// <example>Vender 100 shampoos</example>
    [Required]
    public string Titulo { get; set; } = string.Empty;

    /// <example>Vender 100 shampoos da nova linha "Natura Plant"</example>
    public string Descricao { get; set; } = string.Empty;

    /// <example>100</example>
    [Required]
    public int Xp { get; set; }

    /// <example>50</example>
    [Required]
    public int Moedas { get; set; }
    
    public List<LinkDto> Links { get; set; } = new();
}