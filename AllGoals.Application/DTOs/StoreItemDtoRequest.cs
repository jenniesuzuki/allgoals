using System.ComponentModel.DataAnnotations;

namespace AllGoals.Application.DTOs;

public class StoreItemDtoRequest
{
    /// <example>Um dia de folga extra</example>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <example>Um dia de folga a mais na semana, sendo que o dia é escolhido pela administração</example>
    [StringLength(500, ErrorMessage = "A descrição não pode passar de 500 caracteres.")]
    public string? Descricao { get; set; }

    /// <example>250</example>
    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O valor não pode ser negativo.")]
    public int Valor { get; set; }
}