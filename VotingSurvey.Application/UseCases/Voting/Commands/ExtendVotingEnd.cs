using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Voting.Commands;

public record ExtendVotingEnd : IRequest<ApiResponse<bool>>
{
    public Guid VotingId { get; init; }
    public DateTimeOffset NewEnd { get; init; }
}
