namespace VotingSurvey.Domain.Entities;

public sealed class VotingRecipient
{
    public Guid VotingId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private VotingRecipient() { }

    public VotingRecipient(Guid votingId, Guid userId, DateTimeOffset createdAt)
    {
        VotingId = votingId;
        UserId = userId;
        CreatedAt = createdAt;
    }
}
