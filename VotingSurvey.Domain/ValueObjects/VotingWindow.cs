namespace VotingSurvey.Domain.ValueObjects;

public sealed record VotingWindow
{
    public DateTimeOffset StartsAt { get; init; }
    public DateTimeOffset EndsAt { get; init; }

    // Parameterless constructor for EF Core materialization
    private VotingWindow() { }

    private VotingWindow(DateTimeOffset startsAt, DateTimeOffset endsAt)
    {
        if (startsAt == default) throw new ArgumentException("Start date required", nameof(startsAt));
        if (endsAt == default) throw new ArgumentException("End date required", nameof(endsAt));
        if (startsAt >= endsAt) throw new ArgumentException("Start must be earlier than end");
        StartsAt = startsAt;
        EndsAt = endsAt;
    }

    public static VotingWindow Create(DateTimeOffset startsAt, DateTimeOffset endsAt) => new(startsAt, endsAt);

    public bool HasStarted(DateTimeOffset now) => now >= StartsAt;
    public bool HasEnded(DateTimeOffset now) => now > EndsAt;
    public bool IsActive(DateTimeOffset now) => HasStarted(now) && !HasEnded(now);
    public TimeSpan Remaining(DateTimeOffset now) => EndsAt > now ? EndsAt - now : TimeSpan.Zero;
}
