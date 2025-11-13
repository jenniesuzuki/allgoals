namespace AllGoals.Domain.Entities;

public class Goal
{
    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;

    public int Xp { get; private set; }
    public int Moedas { get; private set; }

    private Goal() 
    { 
    }

    public Goal(string titulo, string descricao, int xp, int moedas)
    {
        AlterarTitulo(titulo);
        AlterarDescricao(descricao);
        AlterarRecompensas(xp, moedas);
    }

    public void AlterarTitulo(string novoTitulo)
    {
        if (string.IsNullOrWhiteSpace(novoTitulo))
            throw new ArgumentException("Título da meta é obrigatório.", nameof(novoTitulo));
            
        Titulo = novoTitulo.Trim();
    }

    public void AlterarDescricao(string novaDescricao)
    {
        Descricao = (novaDescricao ?? string.Empty).Trim();
    }
    public void AlterarRecompensas(int novoXp, int novasMoedas)
    {
        if (novoXp < 0)
            throw new ArgumentException("XP não pode ser negativo.", nameof(novoXp));

        if (novasMoedas < 0)
            throw new ArgumentException("Moedas não podem ser negativas.", nameof(novasMoedas));
            
        if (novoXp == 0 && novasMoedas == 0)
            throw new ArgumentException("A meta deve oferecer pelo menos XP ou Moedas (não pode ser zero).");

        Xp = novoXp;
        Moedas = novasMoedas;
    }
}