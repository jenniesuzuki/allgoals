using System.Text.RegularExpressions;

namespace AllGoals.Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    private Email() { Value = string.Empty; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email não pode ser vazio.", nameof(value));

        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase))
            throw new ArgumentException("Email inválido.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is Email other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}