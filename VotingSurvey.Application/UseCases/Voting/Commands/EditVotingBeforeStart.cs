using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Commands;

public record EditVotingBeforeStart : IRequest<ApiResponse<bool>>
{
    public Guid VotingId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
}
