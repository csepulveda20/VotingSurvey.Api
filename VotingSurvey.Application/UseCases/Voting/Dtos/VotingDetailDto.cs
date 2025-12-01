namespace VotingSurvey.Application.UseCases.Voting.Dtos;

public sealed class VotingDetailDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset StartAt { get; init; }
    public DateTimeOffset EndAt { get; init; }
    public string State { get; init; } = string.Empty; // Upcoming/Active/Closed
    public int YesCount { get; init; }
    public int NoCount { get; init; }
    public string? YourVote { get; init; } // YES/NO/null
}
