using MediatR;
using VotingSurvey.Application.Models;

namespace VotingSurvey.Application.UseCases.Vote.Queries;

public record GetUserVoteStatus : IRequest<ApiResponse<string?>>
{
    public Guid VotingId { get; init; }
    public Guid UserId { get; init; }
}
