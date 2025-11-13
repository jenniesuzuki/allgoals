using System.ComponentModel.DataAnnotations;

namespace AllGoals.Application.DTOs;

public class GoalDtoRequest
{
    /// <example>Vender 100 shampoos</example>
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, MinimumLength = 5, ErrorMessage = "O título deve ter entre 5 e 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    /// <example>Vender 100 shampoos da nova linha "Natura Plant"</example>
    [StringLength(500, ErrorMessage = "A descrição não pode passar de 500 caracteres.")]
    public string? Descricao { get; set; } // Opcional

    /// <example>100</example>
    [Range(0, int.MaxValue, ErrorMessage = "XP não pode ser negativo.")]
    public int Xp { get; set; }

    /// <example>50</example>
    [Range(0, int.MaxValue, ErrorMessage = "Moedas não podem ser negativas.")]
    public int Moedas { get; set; }
}