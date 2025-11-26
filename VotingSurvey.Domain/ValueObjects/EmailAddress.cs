using System.Text.RegularExpressions;

namespace VotingSurvey.Domain.ValueObjects;

public sealed record EmailAddress
{
    private static readonly Regex Pattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    public string Value { get; init; } = string.Empty;

    private EmailAddress() { } // EF Core

    private EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email required", nameof(value));
        value = value.Trim();
        if (value.Length > 256) throw new ArgumentOutOfRangeException(nameof(value));
        if (!Pattern.IsMatch(value)) throw new ArgumentException("Invalid email format", nameof(value));
        Value = value;
    }

    public static EmailAddress Create(string value) => new(value);
    public override string ToString() => Value;
}
