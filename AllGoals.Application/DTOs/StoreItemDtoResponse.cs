using System.ComponentModel.DataAnnotations;

namespace AllGoals.Application.DTOs;

public class StoreItemDtoResponse
{
    /// <example>1</example>
    public int Id { get; set; }

    /// <example>Um dia de folga extra</example>
    [Required]
    public string Nome { get; set; } = string.Empty;

    /// <example>Um dia de folga a mais na semana, sendo que o dia é escolhido pela administração</example>
    public string Descricao { get; set; } = string.Empty;

    /// <example>250</example>
    [Required]
    public int Valor { get; set; }
    
    public List<LinkDto> Links { get; set; } = new();
}