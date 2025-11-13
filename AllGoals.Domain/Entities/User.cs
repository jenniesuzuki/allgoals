using AllGoals.Domain.ValueObjects;

namespace AllGoals.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;

    public bool IsAdmin { get; private set; }

    private User() 
    { 
    } 

    public User(string nome, Email email)
    {
        AlterarNome(nome);
        Email = email ?? throw new ArgumentNullException(nameof(email), "Email obrigatório");
        IsAdmin = false;
    }

    public void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        Nome = nome.Trim();
    }

    public void AlterarEmail(Email novoEmail)
    {
        Email = novoEmail ?? throw new ArgumentNullException(nameof(novoEmail), "Email obrigatório");
    }

    public void PromoverParaAdmin()
    {
        IsAdmin = true;
    }

    public void RevogarAdmin()
    {
        IsAdmin = false;
    }
}