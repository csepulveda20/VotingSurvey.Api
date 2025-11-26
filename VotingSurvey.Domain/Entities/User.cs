using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Domain.Entities;

public sealed class User
{
    public Guid Id { get; }
    public PersonName Name { get; private set; }
    public EmailAddress Email { get; private set; }
    private readonly HashSet<string> _roles = new();
    public IReadOnlyCollection<string> Roles => _roles;
    public bool IsActive { get; private set; }

    private User(Guid id, PersonName name, EmailAddress email, IEnumerable<string> roles, bool isActive)
    {
        Id = id;
        Name = name;
        Email = email;
        _roles = new HashSet<string>(roles);
        IsActive = isActive;
    }

    public static User Create(Guid id, PersonName name, EmailAddress email)
        => new(id, name, email, Enumerable.Empty<string>(), true);

    public void AddRole(string roleCode)
    {
        if (string.IsNullOrWhiteSpace(roleCode)) throw new ArgumentException("Role code is required", nameof(roleCode));
        _roles.Add(roleCode.ToUpperInvariant());
    }

    public bool HasRole(string roleCode) => _roles.Contains(roleCode.ToUpperInvariant());

    public void Deactivate() => IsActive = false;
}
