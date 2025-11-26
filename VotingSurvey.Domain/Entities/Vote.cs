using VotingSurvey.Domain.ValueObjects;

namespace VotingSurvey.Domain.Entities;

public sealed class Vote
{
    public Guid Id { get; }
    public Guid VotingId { get; }
    public Guid UserId { get; }
    public VoteOption Option { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public bool Confirmed { get; private set; }
    public DateTimeOffset? ConfirmedAt { get; private set; }

    private Vote(Guid id, Guid votingId, Guid userId, VoteOption option, DateTimeOffset createdAt)
    {
        Id = id;
        VotingId = votingId;
        UserId = userId;
        Option = option;
        CreatedAt = createdAt;
    }

    public static Vote Select(Guid id, Guid votingId, Guid userId, VoteOption option, DateTimeOffset now)
        => new(id, votingId, userId, option, now);

    public void Confirm(DateTimeOffset now)
    {
        if (Confirmed) throw new InvalidOperationException("Vote already confirmed");
        Confirmed = true;
        ConfirmedAt = now;
    }
}
