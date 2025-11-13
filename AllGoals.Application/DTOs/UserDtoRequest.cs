using System.ComponentModel.DataAnnotations;

namespace AllGoals.Application.DTOs;

public class UserDtoRequest
{
    /// <example>Maria Silva</example>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <example>maria@gmail.com</example>
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
    [StringLength(150, ErrorMessage = "O email não pode exceder 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    /// <example>false</example>
    public bool IsAdmin { get; set; } = false;
}