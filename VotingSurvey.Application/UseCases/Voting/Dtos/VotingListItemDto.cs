namespace VotingSurvey.Application.UseCases.Voting.Dtos;

public sealed class VotingListItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTimeOffset StartAt { get; init; }
    public DateTimeOffset EndAt { get; init; }
    public string State { get; init; } = string.Empty;
}
