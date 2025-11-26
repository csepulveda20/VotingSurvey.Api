namespace VotingSurvey.Domain.ValueObjects;

public sealed record PersonName
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName  { get; init; } = string.Empty;

    private PersonName() { } // EF Core

    private PersonName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name required", nameof(lastName));
        if (firstName.Length > 100) throw new ArgumentOutOfRangeException(nameof(firstName));
        if (lastName.Length > 100) throw new ArgumentOutOfRangeException(nameof(lastName));
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public static PersonName Create(string firstName, string lastName) => new(firstName, lastName);

    public override string ToString() => FirstName + " " + LastName;
}
