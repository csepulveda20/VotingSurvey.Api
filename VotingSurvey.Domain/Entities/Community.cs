namespace VotingSurvey.Domain.Entities;

public sealed class Community
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }

    public Community(Guid id, string name, string? address, bool isActive = true)
    {
        Id = id;
        Name = ValidateName(name);
        Address = address?.Trim();
        IsActive = isActive;
    }

    private static string ValidateName(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Community name required", nameof(value));
        if (value.Length > 200) throw new ArgumentOutOfRangeException(nameof(value));
        return value.Trim();
    }

    public void Deactivate() => IsActive = false;
}
