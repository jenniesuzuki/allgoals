namespace AllGoals.Domain.Entities;

public class StoreItem
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public int Valor { get; private set; }


    private StoreItem() 
    {
    }
    
    public StoreItem(string nome, string descricao, int valor)
    {
        AlterarNome(nome);
        AlterarDescricao(descricao);
        AlterarValor(valor);
    }

    public void AlterarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("Nome do item é obrigatório.", nameof(novoNome));
            
        Nome = novoNome.Trim();
    }

    public void AlterarDescricao(string novaDescricao)
    {
        Descricao = (novaDescricao ?? string.Empty).Trim();
    }

    public void AlterarValor(int novoValor)
    {
        if (novoValor <= 0)
            throw new ArgumentException("O valor do item deve ser positivo (maior que zero).", nameof(novoValor));
        Valor = novoValor;
    }
}