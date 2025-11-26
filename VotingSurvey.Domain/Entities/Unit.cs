namespace VotingSurvey.Domain.Entities;

public sealed class Unit
{
    public Guid Id { get; }
    public Guid CommunityId { get; }
    public string Code { get; private set; }
    public string? Description { get; private set; }

    public Unit(Guid id, Guid communityId, string code, string? description)
    {
        Id = id;
        CommunityId = communityId;
        Code = ValidateCode(code);
        Description = description?.Trim();
    }

    private static string ValidateCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Unit code required", nameof(value));
        if (value.Length > 50) throw new ArgumentOutOfRangeException(nameof(value));
        return value.Trim();
    }
}
